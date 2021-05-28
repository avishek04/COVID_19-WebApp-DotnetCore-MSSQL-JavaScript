const countryNewsUri = 'api/Information/TopCovidNewsCountry/India';
const worldNewsUri = 'api/Information/TopCovidNewsCountry/World';

function getItems() {

    fetch(countryNewsUri)
        .then(response => response.json())
        .then(data => PopulateNewsCard(data, "country"))
        .catch(error => console.error('Unable to get items.', error));

    fetch(worldNewsUri)
        .then(response => response.json())
        .then(data => PopulateNewsCard(data, "world"))
        .catch(error => console.error('Unable to get items.', error));
}

function PopulateNewsCard(data, place) {
    let newsCards = document.getElementsByClassName("topNewsCountry");
    if (place == "world") {
        newsCards = document.getElementsByClassName("topNewsWorld");
    }

    for (let i = 0; i < data.length; i++) {
        const linkTag = document.createElement("a");
        linkTag.href = data[i].url;
        linkTag.innerHTML = data[i].title;

        const headTag = document.createElement("h5");
        headTag.append(linkTag);

        const textTag = document.createElement("p");
        textTag.classList.add('card-text', 'para');
        textTag.innerHTML = data[i].description;

        const subDivTag = document.createElement("div");
        subDivTag.classList.add('card-body', 'btCardNews');
        subDivTag.append(headTag, textTag);

        const imgTag = document.createElement("img");
        imgTag.classList.add('card-img-top', 'imageNews');
        imgTag.src = data[i].urlToImage;

        const mainDivTag = document.createElement("div");
        mainDivTag.classList.add('card', 'newsCountryCard');
        mainDivTag.append(imgTag, subDivTag);

        newsCards[0].append(mainDivTag);
    }
}