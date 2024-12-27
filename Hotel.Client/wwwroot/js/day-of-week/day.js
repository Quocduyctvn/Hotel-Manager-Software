var inpListIdPer = $('#IdDays');
var arrIdPer = [];

var inpDeletedId = $('#DeletedIdDays');
var inpAddedId = $('#AddedIdDays');
var deletedIds = [];
var addedIds = [];

const IS_UPDATE_PAGE = inpDeletedId.length > 0;

if (IS_UPDATE_PAGE) {
	// Ràng buộc dữ liệu đặc biệt cho trang update
	// Nếu xóa hết Days thì không cho lưu
	$.validator.addMethod(
		"deleteAllDays",
		function (value, element) {
			deletedIds.sort();
			arrIdPer.sort();
			if (addedIds.length > 0) {
				return true;
			}
			return !(deletedIds.length === arrIdPer.length && deletedIds.every((value, index) => value === arrIdPer[index]));
		}
	);
	$.validator.unobtrusive.adapters.addBool('deleteAllDays');
}

$(document).ready(function () {
	// Khởi tạo layout masonry
	$('.js-masonry').masonry({
		itemSelector: '.js-group-days',
		columnWidth: '.js-group-days',
		percentPosition: true,
	});

	// Logic khởi tạo trang cập nhật
	if (IS_UPDATE_PAGE) {
		arrIdPer = inpListIdPer.val().split(',');
		arrIdPer.forEach((id) => {
			var checkbox = $(`.check-days[data-id="${id}"]`).prop('checked', true);
			autoChangeCheckAll(checkbox);
		});
	}
});

// Sự kiện thay đổi trạng thái của checkbox
$('.check-days').change(function () {
	let idPer = $(this).attr('data-id');

	if (IS_UPDATE_PAGE) {
		// Xóa item trong mảng added và deleted nếu có
		function removeUpdatedId() {
			let indexInAdded = addedIds.indexOf(idPer);
			if (indexInAdded >= 0) {
				addedIds.splice(indexInAdded, 1);
			}
			let indexInDeleted = deletedIds.indexOf(idPer);
			if (indexInDeleted >= 0) {
				deletedIds.splice(indexInDeleted, 1);
			}
		}

		if (arrIdPer.includes(idPer)) {
			removeUpdatedId();
			if (!$(this).is(':checked')) {
				deletedIds.push(idPer);
			}
		} else {
			removeUpdatedId();
			if ($(this).is(':checked')) {
				addedIds.push(idPer);
			}
		}
		inpDeletedId.val(deletedIds.join(','));
		inpAddedId.val(addedIds.join(','));
	} else {
		// Logic trang thêm mới
		if ($(this).is(':checked') && arrIdPer.indexOf(idPer) === -1) {
			arrIdPer.push(idPer);
		} else {
			arrIdPer.splice(arrIdPer.indexOf(idPer), 1);
		}
		inpListIdPer.val(arrIdPer.join(',')).valid();
	}
	autoChangeCheckAll(this);
});

// Xử lý khi form được reset
$("form").on("reset", function () {
	if (IS_UPDATE_PAGE) {
		setTimeout(() => {
			arrIdPer.forEach((id) => {
				var checkbox = $(`.check-days[data-id="${id}"]`).prop('checked', true);
				autoChangeCheckAll(checkbox);
			});
			deletedIds = [];
			addedIds = [];
		}, 0);
	}
});

// Sự kiện cho checkbox 'chọn tất cả'
$('.check-all').change(function () {
	let parent = $(this).closest('.js-group-days');
	let checkboxes = parent.find('.check-days');
	checkboxes.prop('checked', $(this).is(':checked')).change();
});

// Tự động thay đổi trạng thái của checkbox 'chọn tất cả'
function autoChangeCheckAll(checkbox) {
	let parent = $(checkbox).closest('.js-group-days');
	let checkboxes = parent.find('.check-days');
	let checkAll = parent.find('.check-all');

	checkAll.prop('checked', checkboxes.length === checkboxes.filter(':checked').length);
}
