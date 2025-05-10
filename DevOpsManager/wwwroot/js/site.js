// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function selectRole(roleId) {
    // Обновить значение выбранной роли в форме
    document.getElementById('roleIdInput').value = roleId;

    // Обновить стили, чтобы отобразить, что роль выбрана
    var buttons = document.querySelectorAll('.btn-group-vertical .btn');
    buttons.forEach(button => button.classList.remove('active'));
    var selectedButton = document.querySelector(`[data-role-id="${roleId}"]`);
    selectedButton.classList.add('active');
}
