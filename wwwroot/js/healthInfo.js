const uri = "api/CovidData";

function getItems() {
    let response = fetch(uri);
    response.then(response => response.json()).then(data => displayData(data));
}

function displayData(covidData) {
    const body = document.getElementsByTagName("body");
    body.innerHTML = covidData;
}