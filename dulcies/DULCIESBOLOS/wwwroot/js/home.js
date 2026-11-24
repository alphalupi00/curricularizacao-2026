document.addEventListener("DOMContentLoaded", function () {
    const frames = document.querySelectorAll(".home-cake-frame");
    const anguloMaximo = 14; // Gira até 14 graus para esquerda e direita

    frames.forEach(function (frame) {
        const img = frame.querySelector(".home-cake-img");
        if (!img) return;

        frame.addEventListener("mousemove", function (e) {
            const rect = frame.getBoundingClientRect();
            const xRelativo = ((e.clientX - rect.left) / rect.width - 0.5) * 2;
            const giroY = xRelativo * anguloMaximo;

            img.style.transform = `perspective(900px) rotateY(${giroY.toFixed(2)}deg) scale(1.03)`;
        });

        frame.addEventListener("mouseleave", function () {
            img.style.transform = "perspective(900px) rotateY(0deg) scale(1)";
        });
    });
});