
// Lấy tất cả hình ảnh và các nút
const images = document.querySelectorAll('.big-img-right1 img');
const prevBtn = document.querySelector('.prev1');
const nextBtn = document.querySelector('.next1');

// Khai báo biến để theo dõi hình ảnh hiện tại
let currentIndex = 0;

// Hiển thị hình ảnh đầu tiên
images[currentIndex].classList.add('active');

// Hàm hiển thị hình ảnh theo chỉ số
function showImage(index) {
    images.forEach((img, i) => {
        img.classList.remove('active');
        if (i === index) {
            img.classList.add('active');
        }
    });
}

// Sự kiện khi nhấn nút "prev"
prevBtn.addEventListener('click', () => {
    currentIndex--;
    if (currentIndex < 0) {
        currentIndex = images.length - 1;
    }
    showImage(currentIndex);
});

// Sự kiện khi nhấn nút "next"
nextBtn.addEventListener('click', () => {
    currentIndex++;
    if (currentIndex >= images.length) {
        currentIndex = 0;
    }
    showImage(currentIndex);
});

// ----------------
