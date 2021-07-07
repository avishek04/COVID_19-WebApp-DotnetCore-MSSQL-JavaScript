//const countryNewsUri = 'api/Information/TopCovidNewsCountry/India';
//const worldNewsUri = 'api/Information/TopCovidNewsCountry/World';
const nytNewsUri = 'api/Information/GetTopCovidNewsNYT';
const visitCountUri = 'api/Survey/VistCount/News';

function getItems() {
    //fetch(countryNewsUri)
    //    .then(response => response.json())
    //    .then(data => PopulateNewsCard(data, "country"))
    //    .catch(error => console.error('Unable to get items.', error));


    fetch(visitCountUri)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error("Error", error));

    fetch(nytNewsUri)
        .then(response => response.json())
        .then(data => PopulateNewsCard(data))
        .catch(error => console.error('Unable to get items.', error));

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "V2019N",
        },
        document.getElementsByClassName("topTweetsWorld")[0]
    );

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "WHO",
        },
        document.getElementsByClassName("tweetsWHO")[0]
    );

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "GHS",
        },
        document.getElementsByClassName("tweetsGHS")[0]
    );

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "CDCgov",
        },
        document.getElementsByClassName("topTweetsUS")[0]
    );

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "EU_Health",
        },
        document.getElementsByClassName("topTweetsEurope")[0]
    );

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "AfricaCDC",
        },
        document.getElementsByClassName("topTweetsAfrica")[0]
    );

    twttr.widgets.createTimeline(
        {
            sourceType: "profile",
            screenName: "WHOSEARO",
        },
        document.getElementsByClassName("topTweetsAsia")[0]
    );
}

function PopulateNewsCard(data) {
    //<div class="nyCard">
    //    <div class="newsSection">
    //        <h3><a></a></h3> --
    //        <h4></h4>
    //        <h5></h5>
    //        <h6></h6>
    //    </div>
    //    <img class="imgSection" src=""></img>
    //</div>

    newsCards = document.getElementsByClassName("topNewsWorld");
    for (let i = 0; i < data.length; i++) {
        const linkTag = document.createElement("a");
        linkTag.href = data[i].url;
        linkTag.innerHTML = data[i].title;
        linkTag.target = "_blank";

        const headTag = document.createElement("h4");
        headTag.append(linkTag);

        const textTag = document.createElement("h5");
        textTag.innerHTML = data[i].description;

        const timeTag = document.createElement("h6");
        let dateTime = data[i].publishedAt.split("+");
        timeTag.innerHTML = dateTime[0];

        const newsDiv = document.createElement("div");
        newsDiv.classList.add('newsSection');
        newsDiv.append(headTag, textTag, timeTag);

        const imgTag = document.createElement("img");
        imgTag.classList.add('imgSection');
        imgTag.src = data[i].urlToImage;

        const mainDivTag = document.createElement("div");
        mainDivTag.classList.add('nyCard');
        mainDivTag.append(newsDiv, imgTag);

        newsCards[0].append(mainDivTag);
    }
}