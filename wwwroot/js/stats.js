const uri = 'api/CovidData/CovidGridData';
const chartUri = 'api/CovidData/CovidGraphData';
google.charts.load('current', { 'packages': ['corechart'] });

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => PopulateGridData(data))
        .catch(error => console.error('Unable to get data.', error));

    fetch(chartUri)
        .then(response => response.json())
        .then(data => populateChart(data))
        .catch(error => console.error('Unable to get data.', error));
}

function populateChart(covidData) {
    let countryData = covidData.filter(countryFilter);
    let sortedData = countryData.sort(sortByDate);
    let activeData = ActiveCaseData(sortedData);
    let deathData = DeathCaseData(sortedData)
    google.charts.setOnLoadCallback(drawVisualization);
     
    function drawVisualization() {
        var dataCases = google.visualization.arrayToDataTable(activeData);
        var dataDeaths = google.visualization.arrayToDataTable(deathData);

        var optionsCases = {
            title: 'Active Cases Everyday',
            vAxis: { title: 'Active Cases' },
            hAxis: { title: 'Time' },
            seriesType: 'bars',
            series: { 1: { type: 'line' } }
        };

        var optionsDeaths = {
            title: 'Death Cases Everyday',
            vAxis: { title: 'Death Cases' },
            hAxis: { title: 'Time' },
            seriesType: 'bars',
            series: { 1: { type: 'line' } }
        };

        var chartCases = new google.visualization.ComboChart(document.getElementById('chart_div_cases'));
        var chartDeaths = new google.visualization.ComboChart(document.getElementById('chart_div_deaths'));
        chartCases.draw(dataCases, optionsCases);
        chartDeaths.draw(dataDeaths, optionsDeaths);
    }
}

function ActiveCaseData(sortedData) {
    var activeCase = [['ReportDate', 'Active Cases']];

    sortedData.forEach(x => activeCase.push(
        [x.report_date, x.active_cases]
    ));

    return activeCase;
}

function DeathCaseData(sortedData) {
    var deathCase = [['ReportDate', 'Death Cases']];

    sortedData.forEach(x => deathCase.push(
        [x.report_date, x.new_deaths]
    ));

    return deathCase;
}

function countryFilter(countryObj) {
    if (countryObj.country_name == "India") {
        return true;
    }
}

function sortByDate(obj1, obj2) {
    if (obj1.report_date > obj2.report_date) {
        return 1;
    }
}

function PopulateGridData(data) {
    const table = document.getElementById("table");
    const thr = document.createElement("tr");

    const countryHead = document.createElement("th");
    countryHead.textContent = "Country";
    const totalCaseHead = document.createElement("th");
    totalCaseHead.textContent = "Total Cases";
    const activeCaseHead = document.createElement("th");
    activeCaseHead.textContent = "Active Cases";
    const lastDayCaseHead = document.createElement("th");
    lastDayCaseHead.textContent = "New Cases in 1 day";
    const twoWeekCaseHead = document.createElement("th");
    twoWeekCaseHead.textContent = "New Cases in 14 days"
    const recoveredCaseHead = document.createElement("th");
    recoveredCaseHead.textContent = "Recovered";
    const deathCaseHead = document.createElement("th");
    deathCaseHead.textContent = "Deaths";

    thr.append(countryHead, totalCaseHead, activeCaseHead, lastDayCaseHead, twoWeekCaseHead, recoveredCaseHead, deathCaseHead);
    table.append(thr);

    data.forEach(gridBinding);

    function gridBinding(countryObj) {
        const tr = document.createElement("tr");

        const countryTd = document.createElement("td");
        countryTd.textContent = countryObj.country_name;

        const totalTd = document.createElement("td");
        totalTd.textContent = countryObj.totalCaseCount;

        const activeTd = document.createElement("td");
        activeTd.textContent = countryObj.activeCaseCount;

        const oneTd = document.createElement("td");
        oneTd.textContent = countryObj.lastDayCaseCount;

        const weekTd = document.createElement("td");
        weekTd.textContent = countryObj.lastTwoWeekCaseCount;

        const recoveredTd = document.createElement("td");
        recoveredTd.textContent = countryObj.recoveredCaseCount;

        const deathTd = document.createElement("td");
        deathTd.textContent = countryObj.totalDeathCaseCount;

        tr.append(countryTd, totalTd, activeTd, oneTd, weekTd, recoveredTd, deathTd);
        table.append(tr);
    }
}