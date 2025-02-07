//const sign_in_btn = document.querySelector("#sign_in_btn");
//const sign_up_btn = document.querySelector("#sign_up_btn");
//const container = document.querySelector(".container");

//sign_up_btn.addEventListener("click", () => {
//    container.classList.add("sign-up-mode");
//})

//sign_in_btn.addEventListener("click", () => {
//    container.classList.remove("sign-up-mode");
//})

const sign_in_btn = document.querySelector("#sign_in_btn");
const sign_up_btn = document.querySelector("#sign_up_btn");
const container = document.querySelector(".container");
console.log(container)
sign_up_btn.addEventListener("click", (evt) => {
    evt.preventDefault();
    container.classList.add("sign-up-mode");
    return false;
})

sign_in_btn.addEventListener("click", (evt) => {
    evt.preventDefault();
    container.classList.remove("sign-up-mode");
    return false;
})

document.addEventListener("DOMContentLoaded", function () {

    const form = document.querySelector(".sign-up-form");
    form.addEventListener('submit', function (event) {
        let hasError = false; // Biến theo dõi lỗi

        const surname = document.getElementById('surname').value.trim();
        const phonenumber = document.getElementById('phonenumber').value.trim();
        const password = document.getElementById('password').value.trim();

        // Kiểm tra tên đăng ký
        if (surname.length < 4 || surname.length > 20) {
            alert('Tên của bạn không hợp lệ.');
            hasError = true; // Đánh dấu có lỗi
        }

        // Kiểm tra số điện thoại
        const checkSDT = /^(09|03|07|08|05)[0-9]{8}$/;
        if (phonenumber === '') {
            alert('Bạn chưa điền số điện thoại!');
            hasError = true; // Đánh dấu có lỗi
        } else if (!checkSDT.test(phonenumber)) {
            alert('Số điện thoại của bạn không hợp lệ!');
            hasError = true; // Đánh dấu có lỗi
        }

        // Kiểm tra mật khẩu
        if (password.length < 6) {
            alert('Mật khẩu phải ít nhất 6 ký tự!');
            hasError = true; // Đánh dấu có lỗi
        }

/*        // Nếu có lỗi, ngăn gửi biểu mẫu
        if (hasError) {
            event.preventDefault();
        }*/
    });
});
