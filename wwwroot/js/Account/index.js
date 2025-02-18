const loginPassword = document.querySelector("#loginPassword");
const togglePassword = document.querySelector("#togglePassword");

togglePassword?.addEventListener("click", (e) => {
    e.target.classList.toggle("fa-eye");
    e.target.classList.toggle("fa-eye-slash");
    if (e.target.classList.contains("fa-eye")) {
        loginPassword.setAttribute("type", "text");
    } else {
        loginPassword.setAttribute("type", "password");
    }
});
