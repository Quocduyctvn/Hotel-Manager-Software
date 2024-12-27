$(function () {
	///Tự động ẩn cảnh báo sau 7 giây
	$(".alert.js-alert").delay(10000).slideUp(300, function () {
		$(this).alert('close');
	});

	//Khởi tạo tooltip
	$(".js-tooltip").tooltip();

	$(".bs-autocomplete").bsautocomplete();
});

function convertToDMY(date, splitChar = '/') {
	if (!date) {
		return null;
	}
	if (typeof (date) == 'string') {
		date = new Date(date);
	}
	const yyyy = date.getFullYear();
	let mm = date.getMonth() + 1; // Months start at 0!
	let dd = date.getDate();

	if (dd < 10) dd = '0' + dd;
	if (mm < 10) mm = '0' + mm;

	return dd + splitChar + mm + splitChar + yyyy;
}

function convertToYMD(date, splitChar = '/') {
	if (!date) {
		return null;
	}
	if (typeof (date) == 'string') {
		date = new Date(date);
	}
	const yyyy = date.getFullYear();
	let mm = date.getMonth() + 1; // Months start at 0!
	let dd = date.getDate();

	if (dd < 10) dd = '0' + dd;
	if (mm < 10) mm = '0' + mm;

	return yyyy + splitChar + mm + splitChar + dd;
}


function showLoading() {
	$("#loading").removeClass("d-none");
	$("#wrapper").addClass("app-loading");
}

function hideLoading() {
	setTimeout(function () {
		$("#loading").addClass("d-none");
		$("#wrapper").removeClass("app-loading");
	}, 250);
}

function fillForm(obj, excludeProps = [], parentSelector = null) {
	for (let key in obj) {
		if (excludeProps.indexOf(key) >= 0) {
			continue;
		}
		let eleId = key.charAt(0).toUpperCase() + key.slice(1);
		let eleInput;
		if (parentSelector) {
			eleInput = $(`${parentSelector} [name=${eleId}]`);
		} else {
			eleInput = $(`[name=${eleId}]`);
		}

		if (eleInput.length == 0) continue;

		let eleName = eleInput[0].nodeName;
		let value = obj[key];
		if (eleName == "INPUT") {
			let inputType = eleInput.attr("type");
			switch (inputType) {
				case "date": {
					value = convertToYMD(value, '-');
					eleInput.val(value);
					break;
				}
				case 'radio':
				case 'checkbox': {
					for (let i = 0; i < eleInput.length; i++) {
						let rad = $(eleInput[i]);
						if (rad.val() == value) {
							rad.prop('checked', true);
						}
					}
					break;
				}
				default: {
					eleInput.val(value);
					break;
				}
			}
		} else if (eleName == 'SELECT') {
			eleInput.val(value).change();
		} else if (eleName == 'TEXTAREA') {
			eleInput.val(value);
		}
	}
}



$(document).on("click", ".js-delete-confirm", function (ev) {
	ev.preventDefault();
	let btnDelete = $(this);
	let msg = btnDelete.data('msg');
	let mode = btnDelete.data('mode');
	if (!msg) {
		msg = 'Xác nhận xóa?';
	}

	confirm(msg, () => {
		if (mode == "submit") {
			let form = btnDelete.closest("form");
			if (form.valid()) {
				form.submit();
			}
		}
		else {
			location.href = $(this).attr("href");
		}
	});
});

// Các hàm liên quan xóa nhiều

$("#chkAll").click(function () {
	var isCheckAll = $(this).prop('checked');
	// Tránh trường hợp count sai
	$('td.js-col-checkbox > input').each(function (i, item) {
		var statusBefore = item.checked;
		item.checked = isCheckAll;
		if (item.checked != statusBefore) {
			$(item).change();
		}
	});
});


$("#chkAllowBulkDel").change(function (ev) {
	var isCheck = $(this).is(":checked");
	if (isCheck) {
		$(".js-col-checkbox").removeClass("d-none");
		$("#btnBulkDel").removeClass("d-none");
	} else {
		$(".js-col-checkbox").addClass("d-none");
		$("#btnBulkDel").addClass("d-none");
	}
});

var chkCount = $("td.js-col-checkbox > input").length;
$("td.js-col-checkbox > input").change(function (ev) {
	var isCheck = $(this).is(":checked");
	var delCountEle = $("#delCount");
	var delCountVal = Number(delCountEle.text());
	if (isCheck) {
		delCountVal++;
	} else {
		delCountVal--;
	}
	delCountEle.text(delCountVal);
	$("#chkAll").prop('checked', delCountVal == chkCount);
});

$("#btnBulkDel").click(function (ev) {
	var delCountEle = $("#delCount");
	var delCountVal = Number(delCountEle.text());
	var keyword = $(this).data("keyword");
	var url = $(this).data("url");
	if (!delCountVal) {
		alert(`Chưa chọn ${keyword} để xóa`);
		return;
	}
	confirm(`Xác nhận xóa hàng loạt ${delCountVal} ${keyword}?`, function () {
		var ids = $("td.js-col-checkbox > input:checked").toArray().map(function (i) {
			return i.getAttribute("data-id");
		});
		url = url + "?ids=" + ids.join("&ids=");
		location.assign(url);
	});
});




// ============Amenity model _Update& delete) ====
function UpdateAmenity(name, desc, id) {
	document.getElementById('UdNameAmenity').value = name;
	document.getElementById('UdDescAmenity').value = desc;
	document.getElementById('UdIdAmenity').value = id;
}
function DeleteAmenity(name, id) {
	document.getElementById('DlIdAmenity').value = id;
	document.getElementById('DlNameAmenity').textContent = `[${name}]`;
}

//================= xử lí chọn ảnh RoomCate=====
document.querySelectorAll('.file-input').forEach(input => {
	input.addEventListener('change', function (event) {
		const file = event.target.files[0];
		const previewImage = input.closest('.position-relative').querySelector('.img-preview');
		const removeImageIcon = input.closest('.position-relative').querySelector('.remove-image');

		if (file) {
			const reader = new FileReader();
			reader.onload = function (e) {
				previewImage.src = e.target.result;
				previewImage.style.display = 'block';
				removeImageIcon.style.display = 'block';
			};
			reader.readAsDataURL(file);
		} else {
			previewImage.style.display = 'none';
			removeImageIcon.style.display = 'none';
		}
	});

	const removeImageIcon = input.closest('.position-relative').querySelector('.remove-image');
	removeImageIcon.addEventListener('click', function () {
		input.value = '';
		const previewImage = input.closest('.position-relative').querySelector('.img-preview');
		previewImage.style.display = 'none';
		removeImageIcon.style.display = 'none';
	});
});

//==========================Tyni MCE ============================
tinymce.init({
	selector: '#DescRoomCate',
	plugins: '... image ...',
	toolbar: '... image ...',
});