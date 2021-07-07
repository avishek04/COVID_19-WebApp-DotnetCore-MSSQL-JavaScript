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
    statsDivs[0].innerHTML = data[0].totalCases;
    statsDivs[1].innerHTML = data[0].totalDeaths;
    statsDivs[2].innerHTML = data[0].dailyCases;
    statsDivs[3].innerHTML = data[0].dailyDeaths;
    statsDivs[4].innerHTML = data[0].weeklyCases;
    statsDivs[5].innerHTML = data[0].weeklyDeaths;
    statsDivs[6].innerHTML = data[0].biweeklyCases;
    statsDivs[7].innerHTML = data[0].biweeklyDeaths;
    statsDivs[8].innerHTML = data[1].totalCases;
    statsDivs[9].innerHTML = data[1].totalDeaths;
    statsDivs[10].innerHTML = data[1].dailyCases;
    statsDivs[11].innerHTML = data[1].dailyDeaths;
    statsDivs[12].innerHTML = data[1].weeklyCases;
    statsDivs[13].innerHTML = data[1].weeklyDeaths;
    statsDivs[14].innerHTML = data[1].biweeklyCases;
    statsDivs[15].innerHTML = data[1].biweeklyDeaths;
}

function PopulateVacGridData(data) {
    const countryName = document.getElementById("country");
    const statsDivs = document.getElementsByClassName("vacStat");
    countryName.innerHTML = data[0].countryName;
    statsDivs[0].innerHTML = data[0].totalVaccinations;
    statsDivs[1].innerHTML = data[0].totalVaccinationPerHun;
    statsDivs[2].innerHTML = data[0].peopleVaccinated;
    statsDivs[3].innerHTML = data[0].peopleVaccinatedPerHun;
    statsDivs[4].innerHTML = data[0].peopleFullyVaccinated;
    statsDivs[5].innerHTML = data[0].peopleFullyVaccinatedPerHun;
    statsDivs[6].innerHTML = data[0].dailyVaccinations;
    statsDivs[7].innerHTML = data[0].dailyVaccinationsPerMillion;
    statsDivs[8].innerHTML = data[1].totalVaccinations;
    statsDivs[9].innerHTML = data[1].totalVaccinationPerHun;
    statsDivs[10].innerHTML = data[1].peopleVaccinated;
    statsDivs[11].innerHTML = data[1].peopleVaccinatedPerHun;
    statsDivs[12].innerHTML = data[1].peopleFullyVaccinated;
    statsDivs[13].innerHTML = data[1].peopleFullyVaccinatedPerHun;
    statsDivs[14].innerHTML = data[1].dailyVaccinations;
    statsDivs[15].innerHTML = data[1].dailyVaccinationsPerMillion;
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