const uri = 'api/CovidData/CovidGridData';
const chartUri = 'api/CovidData/CovidGraphData';
const chartAvgUri = 'api/CovidData/CovidAverageGraphData';
const countryUrl = 'api/CountryData/CountryList';
const visitCountUri = 'api/Survey/VistCount/Statistics';

google.charts.load('current', { 'packages': ['corechart'] });
const defaultCountry = "India";
const defaultCountryUrl = `${chartUri}/${defaultCountry}`;

const casesDrpDown1 = document.getElementById("casesCountry1");
const casesDrpDown2 = document.getElementById("casesCountry2");
const caseTypeDrp = document.getElementById("caseType");
const chartTimeCasesDrp = document.getElementById("chartTimeCases");
const deathsDrpDown1 = document.getElementById("deathsCountry1");
const deathsDrpDown2 = document.getElementById("deathsCountry2");
const deathTypeDrp = document.getElementById("deathType");
const chartTimeDeathsDrp = document.getElementById("chartTimeDeaths");

function getItems() {
    fetch(visitCountUri)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error("Error", error));

    fetch(uri)
        .then(response => response.json())
        .then(data => PopulateGridData(data))
        .catch(error => console.error('Unable to get data.', error));

    fetch(countryUrl)
        .then(response => response.json())
        .then(data => PopulateCountryDropDown(data))
        .catch(error => console.error('Unable to get data.', error));

    fetch(defaultCountryUrl)
        .then(response => response.json())
        .then(data => PopulateChart(data))
        .catch(error => console.error('Unable to get data.', error));
}

function PopulateChart(data) {
    PopulateSingleCasesChart(data);
    PopulateSingleDeathsChart(data);
} 

function PopulateSingleCasesChart(covidData) {
    let timePeriod = chartTimeCasesDrp.options[chartTimeCasesDrp.selectedIndex].innerHTML;
    let sortedData = covidData.sort(sortByDate);
    let filteredData = timePeriod === "All time" ? sortedData : FilterByDate(sortedData, timePeriod);
    let activeData = ActiveCaseData(filteredData);
    google.charts.setOnLoadCallback(drawVisualization);
     
    function drawVisualization() {
        var dataCases = google.visualization.arrayToDataTable(activeData);

        var optionsCases = {
            vAxis: { title: 'Positive Cases' },
            hAxis: {
                title: 'Time'
            },
            seriesType: 'bars',
            series: { 1: { type: 'line' } },
            bar: { groupWidth: '70%' },
            legend: { position: 'top' },
            animation: {
                "startup": true,
                duration: 1500,
                easing: 'out',
            }
        };

        var chartCases = new google.visualization.ComboChart(document.getElementById('chart_div_cases'));
        chartCases.draw(dataCases, optionsCases);
    }
}

function PopulateSingleDeathsChart(covidData) {
    let timePeriod = chartTimeDeathsDrp.options[chartTimeDeathsDrp.selectedIndex].innerHTML;
    let sortedData = covidData.sort(sortByDate);
    let filteredData = timePeriod === "All time" ? sortedData : FilterByDate(sortedData, timePeriod);
    let deathData = DeathCaseData(filteredData);
    google.charts.setOnLoadCallback(drawVisualization);

    function drawVisualization() {
        var dataDeaths = google.visualization.arrayToDataTable(deathData);

        var optionsDeaths = {
            vAxis: { title: 'Death Cases' },
            hAxis: { title: 'Time' },
            seriesType: 'bars',
            series: { 1: { type: 'line' } },
            bar: { groupWidth: '70%' },
            legend: { position: 'top' },
            animation: {
                "startup": true,
                duration: 1500,
                easing: 'out',
            }
        };

        var chartDeaths = new google.visualization.ComboChart(document.getElementById('chart_div_deaths'));
        chartDeaths.draw(dataDeaths, optionsDeaths);
    }
}

function PopulateDoubleCasesChart(covidData1, covidData2) {
    let timePeriod = chartTimeCasesDrp.options[chartTimeCasesDrp.selectedIndex].innerHTML;
    let sortedCountry1Data = covidData1.sort(sortByDate);
    let sortedCountry2Data = covidData2.sort(sortByDate);
    let filteredCountry1Data = timePeriod === "All time" ? sortedCountry1Data : FilterByDate(sortedCountry1Data, timePeriod);
    let filteredCountry2Data = timePeriod === "All time" ? sortedCountry2Data : FilterByDate(sortedCountry2Data, timePeriod);
    let chartDataSet = MergeCountryCasesData(filteredCountry1Data, filteredCountry2Data);

    google.charts.setOnLoadCallback(drawVisualization);

    function drawVisualization() {
        var twoCountryCases = google.visualization.arrayToDataTable(chartDataSet);

        var optionsCases = {
            title: 'Cases Comparison Chart',
            height: 400,
            vAxis: { title: 'Positive Cases' },
            hAxis: { title: 'Time' },
            curveType: 'function',
            legend: { position: 'top' },
            animation: {
                "startup": true,
                duration: 1500,
                easing: 'out',
            }
        };

        var chartTwoCountryCases = new google.visualization.LineChart(document.getElementById('chart_div_cases'));
        chartTwoCountryCases.draw(twoCountryCases, optionsCases);
    }
}

function PopulateDoubleDeathsChart(covidData1, covidData2) {
    let timePeriod = chartTimeDeathsDrp.options[chartTimeDeathsDrp.selectedIndex].innerHTML;
    let sortedCountry1Data = covidData1.sort(sortByDate);
    let sortedCountry2Data = covidData2.sort(sortByDate);
    let filteredCountry1Data = timePeriod === "All time" ? sortedCountry1Data : FilterByDate(sortedCountry1Data, timePeriod);
    let filteredCountry2Data = timePeriod === "All time" ? sortedCountry2Data : FilterByDate(sortedCountry2Data, timePeriod);
    let chartDataSet = MergeCountryDeathsData(filteredCountry1Data, filteredCountry2Data);

    google.charts.setOnLoadCallback(drawVisualization);

    function drawVisualization() {
        var twoCountryDeaths = google.visualization.arrayToDataTable(chartDataSet);

        var optionsDeaths = {
            title: 'Deaths Comparison chart',
            height: 400,
            vAxis: { title: 'Death Cases' },
            hAxis: { title: 'Time' },
            curveType: 'function',
            legend: { position: 'top' },
            animation: {
                "startup": true,
                duration: 1500,
                easing: 'out',
            }
        };

        var chartTwoCountryDeaths = new google.visualization.LineChart(document.getElementById('chart_div_deaths'));
        chartTwoCountryDeaths.draw(twoCountryDeaths, optionsDeaths);
    }
}

function MergeCountryCasesData(country1Data, country2Data) {
    let firstCountry = country1Data[0].countryName;
    let secondCountry = country2Data[0].countryName;
    let caseTypeSelected = caseTypeDrp.options[caseTypeDrp.selectedIndex].innerHTML;
    let chartTimeCasesSelected = chartTimeCasesDrp.options[chartTimeCasesDrp.selectedIndex].innerHTML;

    let doubleCountryCase = [['ReportDate', `${firstCountry} ${caseTypeSelected}`, `${secondCountry} ${caseTypeSelected}`]];

    if (caseTypeSelected == "Daily Cases") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryCase.push(
                        [new Date(country1Obj.reportDate), country1Obj.dailyCases, country2Obj.dailyCases]
                    );
                }
            });
        });
    }

    if (caseTypeSelected == "Total Cases") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryCase.push(
                        [new Date(country1Obj.reportDate), country1Obj.totalCases, country2Obj.totalCases]
                    );
                }
            });
        });
    }

    return doubleCountryCase;
}

function MergeCountryDeathsData(country1Data, country2Data) {
    let firstCountry = country1Data[0].countryName;
    let secondCountry = country2Data[0].countryName;
    let deathTypeSelected = deathTypeDrp.options[deathTypeDrp.selectedIndex].innerHTML;
    let chartTimeDeathsSelected = chartTimeDeathsDrp.options[chartTimeDeathsDrp.selectedIndex].innerHTML;

    let doubleCountryDeath = [['ReportDate', `${firstCountry} ${deathTypeSelected}`, `${secondCountry} ${deathTypeSelected}`]];

    if (deathTypeSelected == "Daily Deaths") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryDeath.push(
                        [new Date(country1Obj.reportDate), country1Obj.dailyDeaths, country2Obj.dailyDeaths]
                    );
                }
            });
        });
    }

    if (deathTypeSelected == "Total Deaths") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryDeath.push(
                        [new Date(country1Obj.reportDate), country1Obj.totalDeaths, country2Obj.totalDeaths]
                    );
                }
            });
        });
    }
    
    return doubleCountryDeath;
}

function ActiveCaseData(sortedData) {
    let countryName = sortedData[0].countryName;
    let caseTypeSelected = caseTypeDrp.options[caseTypeDrp.selectedIndex].innerHTML;

    let positiveCase = [['ReportDate', `${countryName} ${caseTypeSelected} Bar Chart`, `${countryName} ${caseTypeSelected} Line Chart`]];

    if (caseTypeSelected == "Daily Cases") {
        sortedData.forEach(x => positiveCase.push(
            [new Date(x.reportDate), x.dailyCases, x.dailyCases]
        ));
    }
    if (caseTypeSelected == "Total Cases") {
        sortedData.forEach(x => positiveCase.push(
            [new Date(x.reportDate), x.totalCases, x.totalCases]
        ));
    }

    return positiveCase;
}

function DeathCaseData(sortedData) {
    let countryName = sortedData[0].countryName;
    let deathTypeSelected = deathTypeDrp.options[deathTypeDrp.selectedIndex].innerHTML;
    let chartTimeDeathsSelected = chartTimeDeathsDrp.options[chartTimeDeathsDrp.selectedIndex].innerHTML;

    let deathCase = [['ReportDate', `${countryName} ${deathTypeSelected} Bar Chart`, `${countryName} ${deathTypeSelected} Line Chart`]];

    if (deathTypeSelected == "Daily Deaths") {
        sortedData.forEach(x => deathCase.push(
            [new Date(x.reportDate), x.dailyDeaths, x.dailyDeaths]
        ));
    }
    if (deathTypeSelected == "Total Deaths") {
        sortedData.forEach(x => deathCase.push(
            [new Date(x.reportDate), x.totalDeaths, x.totalDeaths]
        ));
    }

    return deathCase;
}

function FilterByDate(sortedCovidData, timeSpan) {
    if (timeSpan == "1 week") {
        return sortedCovidData.slice(sortedCovidData.length - 7);
    }
    if (timeSpan == "2 weeks") {
        return sortedCovidData.slice(sortedCovidData.length - 14);
    }
    if (timeSpan == "30 days") {
        return sortedCovidData.slice(sortedCovidData.length - 30);
    }
}

function sortByDate(obj1, obj2) {
    return new Date(obj1.reportDate) - new Date(obj2.reportDate);
}

function PopulateGridData(data) {
    const table = document.getElementById("table");

    data.forEach(gridBinding);

    function gridBinding(countryObj) {
        const tr = document.createElement("tr");

        const countryTd = document.createElement("td");
        countryTd.textContent = countryObj.countryName;

        const totalCasesTd = document.createElement("td");
        totalCasesTd.textContent = countryObj.totalCases;

        const totalDeathsTd = document.createElement("td");
        totalDeathsTd.textContent = countryObj.totalDeaths;

        const dailyCasesTd = document.createElement("td");
        dailyCasesTd.textContent = countryObj.dailyCases;

        const dailyDeathsTd = document.createElement("td");
        dailyDeathsTd.textContent = countryObj.dailyDeaths;

        const weeklyCasesTd = document.createElement("td");
        weeklyCasesTd.textContent = countryObj.weeklyCases;

        const weeklyDeathsTd = document.createElement("td");
        weeklyDeathsTd.textContent = countryObj.weeklyDeaths;

        const biweeklyCasesTd = document.createElement("td");
        biweeklyCasesTd.textContent = countryObj.biweeklyCases;

        const biweeklyDeathsTd = document.createElement("td");
        biweeklyDeathsTd.textContent = countryObj.biweeklyDeaths;

        tr.append(countryTd, totalCasesTd, totalDeathsTd, dailyCasesTd, dailyDeathsTd, weeklyCasesTd, weeklyDeathsTd, biweeklyCasesTd, biweeklyDeathsTd);
        table.append(tr);
    }
}

function sortTable(columnNum, order) {
    let rows, val1, val2, shouldSwitch, i;
    const table = document.getElementById("table");
    let switching = true;

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < (rows.length - 1); i++) {
            val1 = Number(rows[i].getElementsByTagName("td")[columnNum].innerHTML);
            val2 = Number(rows[i + 1].getElementsByTagName("td")[columnNum].innerHTML);
            shouldSwitch = false;
            if ((val1 < val2 && order == 'U') || (val1 > val2 && order == 'D')) {
                shouldSwitch = true;
                break;
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}

function searchCountry() {
    let inputTag = document.getElementById("countrySearch");
    let textInput = inputTag.value.toUpperCase();
    const table = document.getElementById("table");
    const rows = table.rows;

    for (let i = 1; i < rows.length; i++) {
        let countryName = rows[i].getElementsByTagName("td")[0].innerHTML;
        if (countryName.toUpperCase().indexOf(textInput) > -1) {
            rows[i].style.display = "";
        }
        else {
            rows[i].style.display = "none";
        }
    }
}

function PopulateCountryDropDown(data) {
    data.forEach((countryObj, index, countryArr) => {
        const lstElementCases1 = document.createElement("option");
        lstElementCases1.value = countryObj.id;
        lstElementCases1.innerHTML = countryObj.country_name;
        casesDrpDown1.appendChild(lstElementCases1);

        const lstElementCases2 = document.createElement("option");
        lstElementCases2.value = countryObj.id;
        lstElementCases2.innerHTML = countryObj.country_name;
        casesDrpDown2.appendChild(lstElementCases2);

        const lstElementDeaths1 = document.createElement("option");
        lstElementDeaths1.value = countryObj.id;
        lstElementDeaths1.innerHTML = countryObj.country_name;
        deathsDrpDown1.appendChild(lstElementDeaths1);

        const lstElementDeaths2 = document.createElement("option");
        lstElementDeaths2.value = countryObj.id;
        lstElementDeaths2.innerHTML = countryObj.country_name;
        deathsDrpDown2.appendChild(lstElementDeaths2);
    });

    for (let i = 0; i < casesDrpDown1.options.length; i++) {
        if (casesDrpDown1.getElementsByTagName("option")[i].innerHTML == defaultCountry) {
            casesDrpDown1.selectedIndex = i;
        }
        if (deathsDrpDown1.getElementsByTagName("option")[i].innerHTML == defaultCountry) {
            deathsDrpDown1.selectedIndex = i;
        }
    }
}

casesDrpDown1.onchange = GetCasesCountry;
casesDrpDown2.onchange = GetCasesCountry;
caseTypeDrp.onchange = GetCasesCountry;
chartTimeCasesDrp.onchange = GetCasesCountry;
deathsDrpDown1.onchange = GetDeathsCountry;
deathsDrpDown2.onchange = GetDeathsCountry;
deathTypeDrp.onchange = GetDeathsCountry;
chartTimeDeathsDrp.onchange = GetDeathsCountry;

function GetCasesCountry() {
    let firstCountryName = casesDrpDown1.options[casesDrpDown1.selectedIndex].innerHTML;
    let secondCountryName = casesDrpDown2.options[casesDrpDown2.selectedIndex].innerHTML;
    let firstCountryUrl = `${chartUri}/${firstCountryName}`;
    let secondCountryUrl = `${chartUri}/${secondCountryName}`;
  
    if (secondCountryName == "No Country Comparison") {
        fetch(firstCountryUrl)
            .then(response => response.json())
            .then(data => PopulateSingleCasesChart(data))
            .catch(error => console.error('Unable to get data.', error));
    }
    else {
        let data1;
        fetch(firstCountryUrl)
            .then(response => response.json())
            .then(function (data) {
                data1 = data;
                return fetch(secondCountryUrl);
            })
            .then(response => response.json())
            .then(data2 => PopulateDoubleCasesChart(data1, data2))
            .catch(error => console.error('Unable to get data.', error));
    }
}

function GetDeathsCountry() {
    let firstCountryName = deathsDrpDown1.options[deathsDrpDown1.selectedIndex].innerHTML;
    let secondCountryName = deathsDrpDown2.options[deathsDrpDown2.selectedIndex].innerHTML;
    let firstCountryUrl = `${chartUri}/${firstCountryName}`;
    let secondCountryUrl = `${chartUri}/${secondCountryName}`;

    if (secondCountryName == "No Country Comparison") {
        fetch(firstCountryUrl)
            .then(response => response.json())
            .then(data => PopulateSingleDeathsChart(data))
            .catch(error => console.error('Unable to get data.', error));
    }
    else {
        let data1;
        fetch(firstCountryUrl)
            .then(response => response.json())
            .then(function (data) {
                data1 = data;
                return fetch(secondCountryUrl);
            })
            .then(response => response.json())
            .then(data2 => PopulateDoubleDeathsChart(data1, data2))
            .catch(error => console.error('Unable to get data.', error));
    }
}