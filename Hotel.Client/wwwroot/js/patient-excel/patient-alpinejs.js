document.addEventListener("alpine:init", function () {
	Alpine.data('checkbox', () => ({
		isOpen: false,
		checkboxes: [],
		dataIds: '',
		checkAll: false,
		init() {
			this.checkboxes = [];
			// Lấy ra tất cả các checkbox trong phần tử Alpine.js và thêm chúng vào mảng checkboxes
			const checkboxElements = this.$refs.checkboxContainer.querySelectorAll('input[type="checkbox"]:not([id^="IsLastHistoryDone_1_"])');
			this.checkboxes = [...checkboxElements];
		},
		checkAllCheckboxes() {
			this.checkAll = !this.checkAll;
			this.checkboxes = this.checkboxes.map((item) => {
				item.checked = this.checkAll;
				return item;
			});
			this.dataIds = this.getDataIds();
		},
		handleCheckboxClick(checkbox) {
			// Tìm vị trí của checkbox trong mảng checkboxes
			const index = this.checkboxes.findIndex(item => item.id === checkbox.id);

			// Cập nhật giá trị trong mảng checkboxes
			if (index !== -1) {
				this.checkboxes = this.checkboxes.map(item => {
					if (item.id == checkbox.id) {
						item.checked = checkbox.checked;
					}
					return item;
				});
			}
			let isCheckedAll = this.checkboxes.every(item => item.checked);
			if (isCheckedAll && !this.checkAll) {
				this.checkAll = !this.checkAll;
				return;
			}
			if (!isCheckedAll && this.checkAll) {
				this.checkAll = false;
				return;
			}
			this.dataIds = this.getDataIds();
		},
		get countCheckedCheckboxes() {
			return this.checkboxes.filter(checkbox => checkbox.checked).length;
		},
		getDataIds() {
			return this.checkboxes.map(item => {
				if (item.checked) return item.value;
			}).join(",");
		}
	}));
});