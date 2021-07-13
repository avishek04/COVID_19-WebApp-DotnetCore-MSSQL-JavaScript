const uri = 'api/CovidData/CovidCountryGridData/India';
const vacUri = 'api/VaccineData/VaccineCountryGridData/India';
const countryNewsUri = 'api/Information/TopCovidNewsCountry';
const visitCountUri = 'api/Survey/VistCount/Home';
//const worldNewsUri = 'api/Information/TopCovidNewsCountry/World';

function getItems() {
    fetch(visitCountUri)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error("Error", error));

    fetch(countryNewsUri)
        .then(response => response.json())
        .then(data => PopulateNewsCard(data, "country"))
        .catch(error => console.error('Unable to get items.', error));

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "COVIDNewsByMIB",
        },
        document.getElementById("COVIDNewsByMIB")
    );
    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "MoHFW_INDIA",
        },
        document.getElementById("MoHFW_INDIA")
    );
    //fetch(worldNewsUri)
    //    .then(response => response.json())
    //    .then(data => PopulateNewsCard(data, "world"))
    //    .catch(error => console.error('Unable to get items.', error));

    fetch(uri)
        .then(response => response.json())
        .then(data => PopulateGridData(data))
        .catch(error => console.error('Unable to get data.', error));

    fetch(vacUri)
        .then(response => response.json())
        .then(data => PopulateVacGridData(data))
        .catch(error => console.error('Unable to get data.', error));
}

function PopulateGridData(data) {
    const countryName = document.getElementById("country");
    const statsDivs = document.getElementsByClassName("stat");
    countryName.innerHTML = data[0].countryName;
    statsDivs[0].innerHTML = data[0].totalCases != 0 ? data[0].totalCases : "Data is yet to be updated";
    statsDivs[1].innerHTML = data[0].totalDeaths != 0 ? data[0].totalDeaths : "Data is yet to be updated";
    statsDivs[2].innerHTML = data[0].dailyCases != 0 ? data[0].dailyCases : "Data is yet to be updated";
    statsDivs[3].innerHTML = data[0].dailyDeaths != 0 ? data[0].dailyDeaths : "Data is yet to be updated";
    statsDivs[4].innerHTML = data[0].weeklyCases != 0 ? data[0].weeklyCases : "Data is yet to be updated";
    statsDivs[5].innerHTML = data[0].weeklyDeaths != 0 ? data[0].weeklyDeaths : "Data is yet to be updated";
    statsDivs[6].innerHTML = data[0].biweeklyCases != 0 ? data[0].biweeklyCases : "Data is yet to be updated";
    statsDivs[7].innerHTML = data[0].biweeklyDeaths != 0 ? data[0].biweeklyDeaths : "Data is yet to be updated";
    statsDivs[8].innerHTML = data[1].totalCases != 0 ? data[1].totalCases : "Data is yet to be updated";
    statsDivs[9].innerHTML = data[1].totalDeaths != 0 ? data[1].totalDeaths : "Data is yet to be updated";
    statsDivs[10].innerHTML = data[1].dailyCases != 0 ? data[1].dailyCases : "Data is yet to be updated";
    statsDivs[11].innerHTML = data[1].dailyDeaths != 0 ? data[1].dailyDeaths : "Data is yet to be updated";
    statsDivs[12].innerHTML = data[1].weeklyCases != 0 ? data[1].weeklyCases : "Data is yet to be updated";
    statsDivs[13].innerHTML = data[1].weeklyDeaths != 0 ? data[1].weeklyDeaths : "Data is yet to be updated";
    statsDivs[14].innerHTML = data[1].biweeklyCases != 0 ? data[1].biweeklyCases : "Data is yet to be updated";
    statsDivs[15].innerHTML = data[1].biweeklyDeaths != 0 ? data[1].biweeklyDeaths : "Data is yet to be updated";
}

function PopulateVacGridData(data) {
    const countryName = document.getElementById("country");
    const statsDivs = document.getElementsByClassName("vacStat");
    countryName.innerHTML = data[0].countryName;
    statsDivs[0].innerHTML = data[0].totalVaccinations != 0 ? data[0].totalVaccinations : "Data is yet to be updated";
    statsDivs[1].innerHTML = data[0].totalVaccinationPerHun != 0 ? data[0].totalVaccinationPerHun : "Data is yet to be updated";
    statsDivs[2].innerHTML = data[0].peopleVaccinated != 0 ? data[0].peopleVaccinated : "Data is yet to be updated";
    statsDivs[3].innerHTML = data[0].peopleVaccinatedPerHun != 0 ? data[0].peopleVaccinatedPerHun : "Data is yet to be updated";
    statsDivs[4].innerHTML = data[0].peopleFullyVaccinated != 0 ? data[0].peopleFullyVaccinated : "Data is yet to be updated";
    statsDivs[5].innerHTML = data[0].peopleFullyVaccinatedPerHun != 0 ? data[0].peopleFullyVaccinatedPerHun : "Data is yet to be updated";
    statsDivs[6].innerHTML = data[0].dailyVaccinations != 0 ? data[0].dailyVaccinations : "Data is yet to be updated";
    statsDivs[7].innerHTML = data[0].dailyVaccinationsPerMillion != 0 ? data[0].dailyVaccinationsPerMillion : "Data is yet to be updated";
    statsDivs[8].innerHTML = data[1].totalVaccinations != 0 ? data[1].totalVaccinations : "Data is yet to be updated";
    statsDivs[9].innerHTML = data[1].totalVaccinationPerHun != 0 ? data[1].totalVaccinationPerHun : "Data is yet to be updated";
    statsDivs[10].innerHTML = data[1].peopleVaccinated != 0 ? data[1].peopleVaccinated : "Data is yet to be updated";
    statsDivs[11].innerHTML = data[1].peopleVaccinatedPerHun != 0 ? data[1].peopleVaccinatedPerHun : "Data is yet to be updated";
    statsDivs[12].innerHTML = data[1].peopleFullyVaccinated != 0 ? data[1].peopleFullyVaccinated : "Data is yet to be updated";
    statsDivs[13].innerHTML = data[1].peopleFullyVaccinatedPerHun != 0 ? data[1].peopleFullyVaccinatedPerHun : "Data is yet to be updated";
    statsDivs[14].innerHTML = data[1].dailyVaccinations != 0 ? data[1].dailyVaccinations : "Data is yet to be updated";
    statsDivs[15].innerHTML = data[1].dailyVaccinationsPerMillion != 0 ? data[1].dailyVaccinationsPerMillion : "Data is yet to be updated";
}

function PopulateNewsCard(data, place) {
    newsCards = document.getElementById("countryNews");
    for (let i = 0; i < data.length; i++) {
        const linkTag = document.createElement("a");
        linkTag.href = data[i].url;
        linkTag.innerHTML = data[i].title;
        linkTag.target = "_blank";

        const headTag = document.createElement("h3");
        headTag.append(linkTag);

        const sourceTag = document.createElement("span");
        sourceTag.innerHTML = data[i].source;
        sourceTag.style.color = "red";

        const timeTag = document.createElement("span");
        timeTag.innerHTML = ` | ${data[i].publishedAt}`;
        timeTag.style.color = "grey";

        const footTag = document.createElement("h5");
        footTag.append(sourceTag, timeTag);

        const mainDivTag = document.createElement("div");
        mainDivTag.append(headTag, footTag);

        newsCards.append(mainDivTag);
    }
}