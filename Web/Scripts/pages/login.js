var invalidCounter = 0, checked = true;

const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

$(window).on("load", function () {
    $('.btn-forget').on('click',function(e){
        e.preventDefault();
       $('.form-items','.form-content').addClass('hide-it');
       $('.form-sent','.form-content').addClass('show-it');
    });
    $('.btn-tab-next').on('click',function(e){
        e.preventDefault();
        $('.nav-tabs .nav-item > .active').parent().next('li').find('a').trigger('click');
    });
});
$(document).ready(function () {
    $('input[name="Email"]').focus();

    //Enter Key
    $(document).on('keypress', function (e) {
        if (e.which == 13) formsubmit();
    });
});

function register() {
    $('.register').removeClass('hide');
    $('.login').addClass('hide');
    $('.login-title').text('Register new account');
    $('.submitbtn').text('Register');
    $('#loginForm').attr('action', '/Account/Register');
    $('.msg-error').text("");
    $('#loginForm').trigger('reset');
    $('#loginForm span').removeClass('span-invalid').addClass('span-valid');
    $('#loginForm input').removeClass('invalid');
    $('input[name="FirstName"]').focus();

    setMonthSelect();
    setYearSelect();
    setDaySelect();
}

function login() {
    $('.register').addClass('hide');
    $('.login').removeClass('hide');
    $('.login-title').text('Login to account');
    $('.submitbtn').text('Login');
    $('#loginForm').attr('action', '/Account/Login');
    $('.msg-error').text("");
    $('#loginForm').trigger('reset');
    $('#loginForm span').removeClass('span-invalid').addClass('span-valid');
    $('#loginForm input').removeClass('invalid');
    $('input[name="Email"]').focus();
}

function setMonthSelect() {
    $('.month-select').empty();
    $('.month-select').append('<option value="-1">Month</option>')

    for (var i = 0; i < monthNames.length; ++i) {
        var value = String(i + 1).padStart(2, '0');
        $('.month-select').append('<option value="' + value + '">' + monthNames[i] + '</option>');
    }
}

function setYearSelect() {
    $('.year-select').empty();
    $('.year-select').append('<option value="-1">Year</option>');

    //Age range: 10 -> 100
    var currentYear = new Date().getFullYear();
    for (var year = currentYear - 10; year >= currentYear - 100; --year) {
        $('.year-select').append('<option value="' + year + '">' + year + '</option>');
    }
}

function setDaySelect() {
    var currentDay = $('.day-select').val();
    var currentYear = new Date().getFullYear();
    var month = parseInt($('.month-select').val());
    var year = parseInt($('.year-select').val());
    var maxDay = daysInMonth(month, year);

    $('.day-select').empty();
    $('.day-select').append('<option value="-1">Day</option>');

    if (month == 0) ++month;
    if (year == 0) year = currentYear - 10;
    if (currentDay == null) currentDay = -1;
    if (currentDay > maxDay) currentDay = -1;

    for (var day = 1; day <= maxDay; ++day) {
        var value = String(day).padStart(2, '0');
        $('.day-select').append('<option value="' + value + '">' + day + '</option>');
    }

    $('.day-select').val(currentDay);
}

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

function formsubmit() {
    if (validate() == false) return;

    var isLogin = $('#loginForm').attr('action') == '/Account/Login';

    if (isLogin == false) { //Register
        //Set DOB
        var dob = $('.month-select').val() + '/' + $('.day-select').val() + '/' + $('.year-select').val();
        $('input[name="DOB"]').val(dob);
    }

    $('#loginForm').submit();
}

function validate() {
    var isLogin = $('#loginForm').attr('action') == '/Account/Login';
    $('.span-repassword').text('Confirm password is required');
    checked = true;
    invalidCounter = 0;

    if (isLogin) { //Validate Login
        //Email
        validateEmpty("Email");

        //Password
        validateEmpty("Password");
    }
    else { //Validate Register
        //First Name
        validateEmpty("FirstName", "fullname");

        //Last Name
        validateEmpty("LastName", "fullname");

        //Email
        validateEmpty("Email");

        //Day
        validateEmpty("day-select", "dob", true);

        //Month
        validateEmpty("month-select", "dob", true);

        //Year
        validateEmpty("year-select", "dob", true);

        //Gender
        validateEmpty("Gender", "gender", true);

        //Password
        validateEmpty("Password");

        //Confirm Password
        validateEmpty("repassword");
    }

    if (checked && isLogin == false) {
        //Match confirm password
        if ($('input[name="Password"]').val().trim() != $('input[name="repassword"]').val().trim()) {
            $('input[name="repassword"]').addClass('invalid');
            $('.span-repassword').removeClass('span-valid').addClass('span-invalid');
            $('.span-repassword').text('Confirm password not match');
            checked = false;
        } else {
            $('input[name="repassword"]').removeClass('invalid');
            $('.span-repassword').addClass('span-valid').removeClass('span-invalid');
        }
    }

    return checked;
}

function validateEmpty(inputName, spanName, isSelect = false) {
    spanName = spanName ? spanName : inputName.toLowerCase();

    var input = 'input[name="' + inputName + '"]';
    var span = '.span-' + spanName;
    var condition = true;

    if (isSelect) { // select tag
        input = 'select.' + inputName.toLowerCase();
        condition = $(input).val() == '-1';
    } else condition = $(input).val().trim() == '';
        
    if (condition) {
        if (invalidCounter++ == 0)
            $(input).focus();

        $(input).addClass('invalid');
        $(span).removeClass('span-valid').addClass('span-invalid');

        checked = false;
    } else {
        $(input).removeClass('invalid');
        $(span).addClass('span-valid').removeClass('span-invalid');
    }
}