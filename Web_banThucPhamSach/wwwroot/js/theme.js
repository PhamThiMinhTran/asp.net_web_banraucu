window.onload = function () {
    var theme = '@(ViewData["theme"] ?? "BinhThuong")';
    console.log("Current Theme from session:", theme);
    setTheme(theme);
};

function setTheme(theme) {
    const themes = {
        BinhThuong: {
            '--heart--': 'rgb(197, 158, 195)',
            '--heart-hover--': 'rgb(197, 144, 194)',
            '--cart--': 'rgb(137, 190, 137)',
            '--cart-hover--': 'rgb(160, 211, 160)',
            '--TrangChu-hover--': 'rgb(99, 137, 99)',
            '--color-main--': '#4eb060',
            '--background-heart-cart-icon--': '#fff0ee'
        },
        Valentine: {
            '--heart--': 'rgb(255, 99, 132)',
            '--heart-hover--': 'rgb(255, 77, 101)',
            '--cart--': 'rgb(255, 182, 193)',
            '--cart-hover--': 'rgb(255, 160, 170)',
            '--TrangChu-hover--': 'rgb(255, 105, 180)',
            '--color-main--': '#ffcccc',
            '--background-heart-cart-icon--': '#fff0f5'
        },
        Halloween: {
            '--heart--': 'rgb(255, 140, 0)',
            '--heart-hover--': 'rgb(255, 69, 0)',
            '--cart--': 'rgb(128, 0, 128)',
            '--cart-hover--': 'rgb(148, 0, 211)',
            '--TrangChu-hover--': 'rgb(72, 61, 139)',
            '--color-main--': '#333333',
            '--background-heart-cart-icon--': '#ffe4b5'
        },
        Lunar_New_Year: {
            '--heart--': 'rgb(255, 0, 0)',
            '--heart-hover--': 'rgb(255, 69, 0)',
            '--cart--': 'rgb(255, 215, 0)',
            '--cart-hover--': 'rgb(255, 223, 0)',
            '--TrangChu-hover--': 'rgb(218, 165, 32)',
            '--color-main--': '#ffebcd',
            '--background-heart-cart-icon--': '#fff0ee'
        }
    };

    const themeVars = themes[theme];
    if (themeVars) {
        for (const varName in themeVars) {
            document.documentElement.style.setProperty(varName, themeVars[varName]);
        }
    }
}

function changeTheme(theme) {
    console.log("Changing theme to:", theme);
    fetch('/Admin/ChangeTheme', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ theme: theme }) // Gửi theme dưới dạng đối tượng JSON
    }).then(response => {
        if (response.ok) {
            console.log("Theme changed successfully on server.");
            setTheme(theme); // Cập nhật theme ngay lập tức mà không cần reload
        } else {
            console.error("Failed to change theme on server");
        }
    }).catch(error => {
        console.error("Error: ", error);
    });
}
