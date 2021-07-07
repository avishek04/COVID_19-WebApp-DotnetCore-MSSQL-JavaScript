let quesUrl = "api/Survey/SurveyQuestions";
let surveyUrl = "api/Survey/SurveyData";
let respUrl = "api/Survey/SurveyResponse";
let singleQuesRespUrl = "api/Survey/SurveyQuesData";
const visitCountUri = 'api/Survey/VistCount/Survey';

google.charts.load('current', { 'packages': ['corechart'] });

function getItems() {
    fetch(visitCountUri)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error("Error", error));

    fetch(quesUrl)
        .then(response => response.json())
        .then(data => PopulateSurveyQues(data))
        .catch(error => "Couldn't fetch the survey questions");
}

function PopulateSurveyQues(surveyQues) {
    let quesTag = document.getElementsByClassName("question");
    let opt1 = document.getElementsByClassName("option1");
    let opt2 = document.getElementsByClassName("option2");
    let answer = document.getElementsByClassName("answer")

    surveyQues.forEach((quesData, index) => {
        let radioInpt = answer[index].getElementsByTagName("input");
        radioInpt[0].checked = false;
        radioInpt[0].checked = false;

        quesTag[index].innerHTML = quesData.question;
        opt1[index].innerHTML = quesData.option1;
        opt2[index].innerHTML = quesData.option2;
    });
}

function displayResponse(quesNum) {
    let surveyQuesUrl = `${singleQuesRespUrl}/${quesNum}`;
    fetch(surveyQuesUrl)
        .then(response => response.json())
        .then(data => DisplayResponseChart(data, quesNum))
        .catch(error => "Couldn't fetch data for this question");
}

function DisplayResponseChart(ansResponse, quesNum) {
    let quesNumber = parseInt(quesNum[1]);

    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        let legendArr = ['', 'Option 1', 'Option 2', { role: 'annotation' }];
        let chartArray = [legendArr];
        let dataArray = ['', ansResponse.positiveAns, ansResponse.negativeAns, ''];
        chartArray.push(dataArray);

        var data = google.visualization.arrayToDataTable(chartArray);

        var options_fullStacked = {
            isStacked: 'percent',
            height: 90,
            animation: {
                "startup": true,
                duration: 1000,
                easing: 'out',
            },
            legend: { position: 'top', maxLines: 3 },
            hAxis: {
                minValue: 0,
                ticks: [0, .5, 1]
            }
        };

        var chart = new google.visualization.BarChart(document.getElementsByClassName("ansStat")[quesNumber - 1]);
        chart.draw(data, options_fullStacked);
    }
}

const responseObj = {};

function SubmitSurvey() {
    const answerDiv = document.getElementsByClassName("answer");
    const comments = document.getElementById("comments");
    const registerModal = document.getElementById("registerModal");

    for (let i = 0; i < answerDiv.length; i++) {
        let inputTag = answerDiv[i].getElementsByTagName("input");
        let opt1 = inputTag[0];
        let opt2 = inputTag[1];
        let propName = `A${i + 1}`;

        if (opt1.checked) {
            responseObj[propName] = true;
            continue;
        }
        if (opt2.checked) {
            responseObj[propName] = false;
            continue;
        }
        alert("Please answer all the questions to submit the survey");
        return 1;
    }

    responseObj["Comments"] = comments.value;
    document.getElementById("email").value = "";
    registerModal.style.display = "block";
}

function RegisterUser() {
    const email = document.getElementById("email");
    const emailId = email.value;
    if (emailId != "" && emailId.indexOf('@') > 0 && emailId.indexOf('.') > 2) {
        responseObj["Email"] = email.value;
        SaveSurveyResponse(responseObj);
    }
    else {
        alert("Please provide a correct Email Id");
    }
}

function SaveSurveyResponse(surveyResponse) {
    fetch(respUrl, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(surveyResponse),
    })
        .then(response => response.json())
        .then(data => ShowNotification(data))
        .catch(error => console.log("no response from API"));
}

function ShowNotification(responseMsg) {
    if (responseMsg == "Success") {
        registerModal.style.display = "none";
        const surveyPage = document.getElementById("survey");
        surveyPage.innerHTML = "<h3 style='margin:10px'>Thank you for giving your time. Your response to the Survey has been saved.</h3>";
    }
    else {
        alert("Please resubmit your response as this email id has already been used for response.");
    }
}

function ClosePopup() {
    registerModal.style.display = "none";
}