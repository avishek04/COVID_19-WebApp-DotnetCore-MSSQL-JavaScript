const vaccineUri = 'api/VaccineData/VaccineGridData';
const vaccineChartUri = 'api/VaccineData/VaccineGraphData';
const countryUrl = 'api/CountryData/CountryList';
const visitCountUri = 'api/Survey/VistCount/Vaccine Stats';

google.charts.load('current', { 'packages': ['corechart'] });
const defaultCountry = "India";
const vacCountryGraphUrl = `${vaccineChartUri}/${defaultCountry}`;

const vacDrpDown1 = document.getElementById("vaccinesCountry1");
const vacDrpDown2 = document.getElementById("vaccinesCountry2");
const vacTypeDrp = document.getElementById("vaccineType");
const chartTimeVacDrp = document.getElementById("chartTimeVaccines");

function getItems() {
    fetch(visitCountUri)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error("Error", error));

    fetch(vaccineUri)
        .then(response => response.json())
        .then(data => PopulateGridData(data))
        .catch(error => console.error('Unable to get data.', error));

    fetch(countryUrl)
        .then(response => response.json())
        .then(data => PopulateCountryDropDown(data))
        .catch(error => console.error('Unable to get data.', error));

    fetch(vacCountryGraphUrl)
        .then(response => response.json())
        .then(data => PopulateChart(data))
        .catch(error => console.error('Unable to get data.', error));
}

function PopulateChart(data) {
    PopulateSingleVacChart(data);
}

function PopulateSingleVacChart(vacData) {
    let timePeriod = chartTimeVacDrp.options[chartTimeVacDrp.selectedIndex].innerHTML;
    let vacTypeSelected = vacTypeDrp.options[vacTypeDrp.selectedIndex].innerHTML;
    let sortedData = vacData.sort(sortByDate);
    let filteredData = timePeriod === "All time" ? sortedData : FilterByDate(sortedData, timePeriod);
    let vaccineData = ActiveCaseData(filteredData);
    google.charts.setOnLoadCallback(drawVisualization);

    function drawVisualization() {
        var dataCases = google.visualization.arrayToDataTable(vaccineData);

        var optionsCases = {
            vAxis: { title: 'Vaccinated' },
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

        var chartVac = new google.visualization.ComboChart(document.getElementById('chart_div_vaccines'));

        if (vacTypeSelected == "Single vs Double Dose") {
            delete optionsCases.bar;
            delete optionsCases.series;
            delete optionsCases.seriesType;
            chartVac = new google.visualization.AreaChart(document.getElementById('chart_div_vaccines'));
        }
        
        chartVac.draw(dataCases, optionsCases);
    }
}

function PopulateDoubleVacChart(vacData1, vacData2) {
    let timePeriod = chartTimeVacDrp.options[chartTimeVacDrp.selectedIndex].innerHTML;
    let sortedCountry1Data = vacData1.sort(sortByDate);
    let sortedCountry2Data = vacData2.sort(sortByDate);
    let filteredCountry1Data = timePeriod === "All time" ? sortedCountry1Data : FilterByDate(sortedCountry1Data, timePeriod);
    let filteredCountry2Data = timePeriod === "All time" ? sortedCountry2Data : FilterByDate(sortedCountry2Data, timePeriod);
    let chartDataSet = MergeCountryVacData(filteredCountry1Data, filteredCountry2Data);

    google.charts.setOnLoadCallback(drawVisualization);

    function drawVisualization() {
        var twoCountryCases = google.visualization.arrayToDataTable(chartDataSet);

        var optionsCases = {
            title: 'Vaccine Comparison Chart',
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

        var chartTwoCountryCases = new google.visualization.LineChart(document.getElementById('chart_div_vaccines'));
        chartTwoCountryCases.draw(twoCountryCases, optionsCases);
    }
}

function MergeCountryVacData(country1Data, country2Data) {
    let firstCountry = country1Data[0].countryName;
    let secondCountry = country2Data[0].countryName;
    let vacTypeSelected = vacTypeDrp.options[vacTypeDrp.selectedIndex].innerHTML;
    let chartTimeVacSelected = chartTimeVacDrp.options[chartTimeVacDrp.selectedIndex].innerHTML;

    let doubleCountryVac = [['ReportDate', `${firstCountry} ${vacTypeSelected}`, `${secondCountry} ${vacTypeSelected}`]];

    if (vacTypeSelected == "Daily Vaccination") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryVac.push(
                        [new Date(country1Obj.reportDate), country1Obj.dailyVaccinations, country2Obj.dailyVaccinations]
                    );
                }
            });
        });
    }

    if (vacTypeSelected == "Fully Vaccinated") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryVac.push(
                        [new Date(country1Obj.reportDate), country1Obj.peopleFullyVaccinated, country2Obj.peopleFullyVaccinated]
                    );
                }
            });
        });
    }

    if (vacTypeSelected == "Total Vaccination") {
        country1Data.forEach((country1Obj, index1) => {
            country2Data.forEach((country2Obj, index2) => {
                if (country1Obj.reportDate == country2Obj.reportDate) {
                    doubleCountryVac.push(
                        [new Date(country1Obj.reportDate), country1Obj.totalVaccinations, country2Obj.totalVaccinations]
                    );
                }
            });
        });
    }   

    return doubleCountryVac;
}

function ActiveCaseData(sortedData) {
    let countryName = sortedData[0].countryName;
    let vacTypeSelected = vacTypeDrp.options[vacTypeDrp.selectedIndex].innerHTML;

    let vacDose = [['ReportDate', `${countryName} ${vacTypeSelected} Bar Chart`, `${countryName} ${vacTypeSelected} Line Chart`]];

    if (vacTypeSelected == "Daily Vaccination") {
        sortedData.forEach(x => vacDose.push(
            [ new Date(x.reportDate), x.dailyVaccinations, x.dailyVaccinations ]
        ));
    }

    if (vacTypeSelected == "Total Vaccination") {
        sortedData.forEach(x => vacDose.push(
            [new Date(x.reportDate), x.totalVaccinations, x.totalVaccinations]
        ));
    }

    if (vacTypeSelected == "Single vs Double Dose") {

        vacDose = [['ReportDate', `First Dose Area Chart`, `Fully Vaccinated Area Chart`]];
        sortedData.forEach(x => vacDose.push(
            [new Date(x.reportDate), x.peopleVaccinated, x.peopleFullyVaccinated]
        ));
    }

    return vacDose;
}

function FilterByDate(sortedVacData, timeSpan) {
    if (timeSpan == "1 week") {
        return sortedVacData.slice(sortedVacData.length - 7);
    }
    if (timeSpan == "2 weeks") {
        return sortedVacData.slice(sortedVacData.length - 14);
    }
    if (timeSpan == "30 days") {
        return sortedVacData.slice(sortedVacData.length - 30);
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

        const totalVacTd = document.createElement("td");
        totalVacTd.textContent = countryObj.totalVaccinations;

        const totalVacPerHdrTd = document.createElement("td");
        totalVacPerHdrTd.textContent = countryObj.totalVaccinationPerHun;

        const frstDoseVacTd = document.createElement("td");
        frstDoseVacTd.textContent = countryObj.peopleVaccinated;

        const frstDoseVacPerHdrTd = document.createElement("td");
        frstDoseVacPerHdrTd.textContent = countryObj.peopleVaccinatedPerHun;

        const secndDoseVacTd = document.createElement("td");
        secndDoseVacTd.textContent = countryObj.peopleFullyVaccinated;

        const secndDoseVacPerHdrTd = document.createElement("td");
        secndDoseVacPerHdrTd.textContent = countryObj.peopleFullyVaccinatedPerHun;

        const dailyVacTd = document.createElement("td");
        dailyVacTd.textContent = countryObj.dailyVaccinations;

        const dailyVacPerMilTd = document.createElement("td");
        dailyVacPerMilTd.textContent = countryObj.dailyVaccinationsPerMillion;

        tr.append(countryTd, totalVacTd, totalVacPerHdrTd, frstDoseVacTd, frstDoseVacPerHdrTd, secndDoseVacTd, secndDoseVacPerHdrTd, dailyVacTd, dailyVacPerMilTd);
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
        const lstElementVac1 = document.createElement("option");
        lstElementVac1.value = countryObj.id;
        lstElementVac1.innerHTML = countryObj.country_name;
        vacDrpDown1.appendChild(lstElementVac1);

        const lstElementVac2 = document.createElement("option");
        lstElementVac2.value = countryObj.id;
        lstElementVac2.innerHTML = countryObj.country_name;
        vacDrpDown2.appendChild(lstElementVac2);
    });

    for (let i = 0; i < vacDrpDown1.options.length; i++) {
        if (vacDrpDown1.getElementsByTagName("option")[i].innerHTML == defaultCountry) {
            vacDrpDown1.selectedIndex = i;
        }
    }
}

vacDrpDown1.onchange = GetVacCountry;
vacDrpDown2.onchange = GetVacCountry;
vacTypeDrp.onchange = GetVacCountry;
chartTimeVacDrp.onchange = GetVacCountry;

function GetVacCountry() {
    let firstCountryName = vacDrpDown1.options[vacDrpDown1.selectedIndex].innerHTML;
    let secondCountryName = vacDrpDown2.options[vacDrpDown2.selectedIndex].innerHTML;
    let firstCountryUrl = `${vaccineChartUri}/${firstCountryName}`;
    let secondCountryUrl = `${vaccineChartUri}/${secondCountryName}`;

    if (secondCountryName == "No Country Comparison") {
        vacTypeDrp.getElementsByTagName("option")[1].innerHTML = "Single vs Double Dose";
        fetch(firstCountryUrl)
            .then(response => response.json())
            .then(data => PopulateSingleVacChart(data))
            .catch(error => console.error('Unable to get data.', error));
    }
    else {
        vacTypeDrp.getElementsByTagName("option")[1].innerHTML = "Fully Vaccinated";
        let data1;
        fetch(firstCountryUrl)
            .then(response => response.json())
            .then(function (data) {
                data1 = data;
                return fetch(secondCountryUrl);
            })
            .then(response => response.json())
            .then(data2 => PopulateDoubleVacChart(data1, data2))
            .catch(error => console.error('Unable to get data.', error));
    }
}