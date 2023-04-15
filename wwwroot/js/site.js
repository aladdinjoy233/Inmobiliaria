// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
setTimeout(() => {
	var divToHide = document.getElementById("notification");
	if (divToHide) {
		divToHide.style.display = "none";
		divToHide.remove();
	}
}, 7000);

var closeWarning = document.getElementById("close-warning-btn");
if (closeWarning) {
	closeWarning.addEventListener("click", () => {
		var divToHide = document.getElementById("info-notification");
		if (divToHide) {
			divToHide.style.display = "none";
			divToHide.remove();
		}
	});
}