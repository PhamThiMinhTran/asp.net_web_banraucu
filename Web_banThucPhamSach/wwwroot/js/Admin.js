function showContent(id) {
    var contents = document.querySelectorAll('.content > div');
    contents.forEach(function (content) {
        content.classList.add('hidden');
    });
    document.getElementById(id).classList.remove('hidden');
}

var ctx = document.getElementById('revenuePieChart').getContext('2d');
var revenuePieChart = new Chart(ctx, {
    type: 'pie',
    data: {
        labels: ['01/09', '05/09', '10/09', '15/09'],
        datasets: [{
            label: 'Doanh Thu Tháng 9',
            data: [20000000, 15000000, 25000000, 30000000],
            backgroundColor: ['#ff6384', '#36a2eb', '#cc65fe', '#ffce56'],
            hoverOffset: 4
        }]
    }
});


