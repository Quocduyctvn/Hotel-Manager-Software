
// chức năng confirm data
$('.js-comfirm-data').click(function (ev) {
	ev.preventDefault();

	var url = $(this).attr('href');
	var data = $('.form-search').serialize();

	if ($("#PagerTotalItemCount").val() == 0) {
		SnackBar.danger('Không có dữ liệu để chuyển đổi', 10000, 'top-center');
		return;
	}
	$.get("/CompanyPatientExcel/IsSameCompany", data)
		.then((isSameCompany) => {
			// Chi cho phép chuyển đổi khi dữ liệu cùng thuộc về một công ty
			if (isSameCompany) {
				confirm("Xác nhận chuyển dữ liệu?", function () {
					$.ajax({
						type: 'GET',
						url,
						data,
						beforeSend: function (jqXHR, settings) {
							showLoading();
						},
						success: function (res) {
							if (!res.success) {
								SnackBar.danger(res.message, 10000, 'top-center');
								return;
							}
							window.location.href = window.location.href;
						},
						error: function (res) {
							SnackBar.danger('Đã xảy ra lỗi trong quá trình xử lý dữ liệu', 10000, 'top-center');
						},
						complete: function () {
							hideLoading();
						}
					});
				});
			} else {
				SnackBar.danger('Dữ liệu công ty không giống nhau, không thể chuyển đổi!', 10000, 'top-center');
			}
		});

	
});