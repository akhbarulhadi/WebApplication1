document.addEventListener('DOMContentLoaded', function () {

    // --- Bar Chart ---
    const barCanvas = document.getElementById('barChart');
    if (barCanvas && barCanvas.dataset.chartModel && barCanvas.dataset.chartModel !== "[]") {
        // 1. Ambil data dari atribut 'data-chart-model'
        const barParse = JSON.parse(barCanvas.dataset.chartModel);

        // 2. Olah data mentah menjadi format yang dibutuhkan Chart.js
        // Penting: Nama properti C# (PascalCase) menjadi camelCase di JSON
        const labels = barParse.map(item => item.DepartmentNm);
        const dataValues = barParse.map(item => item.TotalEmployees);
        new Chart(barCanvas, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Jumlah Karyawan',
                    data: dataValues,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            },
        });
    }

});