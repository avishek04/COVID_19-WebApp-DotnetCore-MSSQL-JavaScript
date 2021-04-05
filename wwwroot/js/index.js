//document.addEventListener("DOMContentLoaded", OnLoadOfDOM);

function getItem() {
	fetch('https://newsapi.org/v2/top-headlines?q=covid&language=en&apiKey=a3916796d2c5440987fd9c732915ee1b')
		.then(response => response.json)
		.then(data => console.log(data));
}
//function getCoordinates() {
//	let location = new Object();
//	if ('geolocation' in navigator) {
//		navigator.geolocation.getCurrentPosition(function(position) {
//			location['lat'] = position.coords.latitude;
//			location['lon'] = position.coords.longitude;
//		});
//	}
//	else {
//		location = {
//			lat: 17.387140,
//			lon: 78.491684
//		}
//	}
//	return location;
//}

//let defaultState = 'Telangana';

//function getAddress() {
//	let path = 'https://forward-reverse-geocoding.p.rapidapi.com/v1/reverse?lat=41.8755616&lon=-87.6244212&format=json&accept-language=en&polygon_threshold=0.0';
//	fetch(path, {
//		"method": "GET",
//		"headers": {
//				"x-rapidapi-key": "1e16512788mshf492a845885b640p1ff150jsn2f0e1d75eeb8",
//				"x-rapidapi-host": "forward-reverse-geocoding.p.rapidapi.com"
//			}
//	}).then(response => response.json())
//		.then(data => function (data) {
//			let address = new Object();
//			address['country'] = data.address.country;
//			address['country_code'] = data.address.country_code;
//			address['state'] = data.address.state ? data.address.state : defaultState;
//			return address;
//		});
//}

//let address = getAddress();

//function getCountryCovidNews(country) {
//	let path = 'https://newsapi.org/v2/top-headlines?country=in&q=covid&language=en&apiKey=a3916796d2c5440987fd9c732915ee1b';
//	let data = fetch(path).then(response => response.json());
//	if (data.status == 'ok') {
//		return data.articles;
//    }
//}

//function getWorldCovidNews() {
//	let path = 'https://newsapi.org/v2/top-headlines?q=covid&language=en&apiKey=a3916796d2c5440987fd9c732915ee1b';
//	let data = fetch(path).then(response => response.json());
//	if (data.status == 'ok') {
//		return data.articles;
//	}
//}

//function populateCovidNews(address) {
//	let newsCardImage = document.getElementsByClassName('card-img-top');
//	let newsCardTitle = document.getElementsByClassName('card-title');
//	let newsCardText = document.getElementsByClassName('card-text');
//	let newsCardLink = document.getElementsByClassName('newsLink');
// 	let articlesCountry;
//	let articlesWorld;

//	if ('country_code' in address) {
//		articlesCountry = getCountryCovidNews(address['country_code']);
//		articlesWorld = getWorldCovidNews();
//	}
//	else {
//		articlesWorld = getWorldCovidNews();
//	}

//	if (articlesCountry.length > 0 && articlesWorld.length > 0) {
//		for (let i = 0; i < 10; i++) {
//			let newsDetails;
//			if (i < 5) {
//				newsDetails = articlesCountry[i];
//			}
//			else {
//				newsDetails = articlesWorld[i - 5];
//			}
//			newsCardImage[i].setAttribute("src", newsDetails.urlToImage);
//			newsCardTitle[i].textContent = newsDetails.title;
//			newsCardText[i].textContent = newsDetails.description;
//			newsCardLink[i].setAttribute('href', newsDetails.url);
//		}
//	}
//	else if (articlesWorld.length > 0) {
//		for (let i = 0; i < 10; i++) {
//			let newsDetails;
//			newsDetails = articlesWorld[i];
//			newsCardImage[i].setAttribute("src", newsDetails.urlToImage);
//			newsCardTitle[i].textContent = newsDetails.title;
//			newsCardText[i].textContent = newsDetails.description;
//			newsCardLink[i].setAttribute('href', newsDetails.url);
//		}
//	}
//	else {
//		for (let i = 0; i < 10; i++) {
//			let newsDetails;
//			newsDetails = articlesCountry[i];
//			newsCardImage[i].setAttribute("src", newsDetails.urlToImage);
//			newsCardTitle[i].textContent = newsDetails.title;
//			newsCardText[i].textContent = newsDetails.description;
//			newsCardLink[i].setAttribute('href', newsDetails.url);
//		}
//    }
//}

//console.log(addressDetails);

//let coordinates = getCoordinates();
//let state = getAddress(coordinates);


//var url = 'http://newsapi.org/v2/top-headlines?country=us&apiKey=a3916796d2c5440987fd9c732915ee1b';

