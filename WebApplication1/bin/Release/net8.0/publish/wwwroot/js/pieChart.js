document.addEventListener('DOMContentLoaded', function () {

    // --- Pie Chart ---
    const ctxPie = document.getElementById('pieChart');
    if (ctxPie) {
        const rawPieData = JSON.parse(ctxPie.dataset.chartModel);

        const labels = rawPieData.map(item => item.GenderNm);
        const dataValues = rawPieData.map(item => item.TotalGenders);
        new Chart(ctxPie, {
            type: 'pie',
            data: {
                labels: labels,
                datasets: [{
                    label: 'My First Dataset',
                    data: dataValues,
                    backgroundColor: [
                        'rgb(54, 162, 235)', // --> Red
                        'rgb(255, 99, 132)',
                        'rgb(255, 205, 86)'
                    ],
                    hoverOffset: 4
                }]
            },
        });
    }
});