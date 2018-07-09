

function LoadCharts(model) {

    LoadGradesChart(model.LatestStudentGrades);
    CreateModuelCharts(model.StudentModules, model.ModuleCourseWork, model.StudentGrades)
}

function CreateModuelCharts(modules, coursWork, grades)
{
    for (i = 0; i < modules.length; i++) {
        var ctx = document.getElementById(modules[i].ModuleShortName).getContext('2d');
        var newLabels = coursWork.map(function (elem) {
            if (modules[i].ModuleID == elem.ModuleID)
                return elem.Title;
        }).filter(function (n) { return n != undefined });

        var newData = grades.map(function (elem) {
            if (newLabels.indexOf(elem.CourseWorkTitle) > -1) {
                return elem.Grade;
            }
        }).filter(function (n) { return n != undefined });

        var newBackgroundColors = GetChartBackgroundColours(grades, newLabels, true);
        var newBorderColours = GetChartBorderColours(grades, newLabels, true);

        CreateChart('horizontalBar', newLabels, newData, newBackgroundColors, newBorderColours, ctx);
    }
}

function LoadGradesChart(latestStudentGrades)
{
    var ctx = document.getElementById("myChart").getContext('2d');
    var newLabels = latestStudentGrades.map(function (elem) {return elem.ModuleShortName;});
    var newData = latestStudentGrades.map(function (elem) { return elem.Grade;});
    var newBackgroundColors = GetChartBackgroundColours(latestStudentGrades);
    var newBorderColours = GetChartBorderColours(latestStudentGrades);

    CreateChart('bar', newLabels, newData, newBackgroundColors, newBorderColours, ctx)
}

function GetChartBackgroundColours(data, labels = null, checkTitle = false)
{
    var newBackgroundColors = data.map(function (elem) {
        if (((checkTitle) && (labels.indexOf(elem.CourseWorkTitle) > -1)) || (!checkTitle)) {
            if (elem.Mark == "F") {
                return 'rgba(255, 99, 71, 0.2)'
            }
            else if (elem.Mark == "P") {
                return 'rgba(119, 221, 119, 0.2)'
            }
            else if (elem.Mark == "M") {
                return 'rgba(0, 0, 255, 0.2)'
            }
            else return 'rgba(255, 255, 0, 0.2)'
        }
    });
    return newBackgroundColors.filter(function (n) { return n != undefined });
}

function GetChartBorderColours(data, labels = null, checkTitle = false)
{
    var newBorderColours = data.map(function (elem) {
        if (((checkTitle) && (labels.indexOf(elem.CourseWorkTitle) > -1)) || (!checkTitle)) {
            if (elem.Mark == "F") {
                return 'rgba(255, 99, 71,1)'
            }
            else if (elem.Mark == "P") {
                return 'rgba(119, 221, 119, 1)'
            }
            else if (elem.Mark == "M") {
                return 'rgba(0, 0, 255, 1)'
            }
            else return 'rgba(255, 255, 0, 1)'
        }
    });

    return newBorderColours.filter(function (n) { return n != undefined });
}

function CreateChart(chartType, chartlabels, chartdata, backgroundColours, borderColours, ctx)
{
    var myChart = new Chart(ctx, {
        type: chartType,
        data: {
            labels: chartlabels,
            datasets: [{
                label: 'Grade Out Of 100',
                data: chartdata,
                backgroundColor: backgroundColours,
                borderColor: borderColours,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    ticks: {
                        beginAtZero: true,
                        max: 100
                    }
                }]
            },
            legend: {
                display: false
            }
        }
    });
    myChart.responsive = true;
    myChart.maintainAspectRatio = true;
}
