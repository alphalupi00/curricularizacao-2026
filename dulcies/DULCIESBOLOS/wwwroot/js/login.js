document.addEventListener("DOMContentLoaded", function () {
    var btn = document.getElementById("toggleSenhaBtn");
    var input = document.getElementById("senha");

    if (btn && input) {
        btn.addEventListener("click", function () {
            var isPassword = input.type === "password";
            input.type = isPassword ? "text" : "password";
            btn.textContent = isPassword ? "Ocultar" : "Mostrar";
        });
    }
});