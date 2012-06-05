$(function () {
    SetNavClass('#mainNav', 'active');
    SetNavClass('.sideNav', 'active');
    SetJqueryUI();
});

function SetNavClass(item, className) {
    var controller = $('#controller').val();
    var action = $('#action').val();
    eval('controller_' + controller + ' = true');
    eval('action_' + action + ' = true');
    list = $(item + ' *');

    for (var k = 0; k < list.length; k++) {
        item = list[k];
        str = GetClassName(item);
        try {
            if (eval(str.toLowerCase())) $(item).addClass(className);
        } catch (e) { }
    }
}
function GetClassName(item) {
    var classStr = $(item).attr('class');
    if (classStr == null) return "";
    classes = classStr.split(' ');
    for (var k = 0; k < classes.length; k++) {
        if (classes[k].indexOf('controller') > -1 || classes[k].indexOf('action') > -1) return classes[k];
    }
}

function SetUrl() {
    var data = $('#ajaxUrl');
    var url = data.val();
    if (url == null) return;
    history.pushState({}, "", url);
}

function UserRoleSelect() {
    if ($('#userrole_all').is(':checked')) {
        $('.userrole').attr('checked', true);
    }
    else {
        $('.userrole').removeAttr('checked');
    }
}

function SetJqueryUI() {
    $("input:checkbox,input:file,input:radio,input:submit,button").button();
    $(".datepicker").datepicker();
}

function ConfirmDelete() {
    return confirm('确认删除?');
}


var login_html;
function LoginForm() {
    $('#username').val('');
    $('#password').val('');
    login_html = $("#login_html").dialog({
        modal: true
    });
}
function Login(url) {
    $.post(url, { username: $('#username').val(), password: $('#password').val() },
		function (data, textStatus) {
		    login_html.dialog('destroy');
		    if (data.Result == true) {
		        $('#login').hide();
		        $('#logout').show();
		        alert('登录成功！');
		    }
		    else {
		        alert('登录失败！');
		    }
		}, "json");
}

function Logout(url) {
    $.post(url, {},
		function (data, textStatus) {
		    $('#logout').hide();
		    $('#login').show();
		    alert('退出成功！');
		}, "json");
}

function CheckIn(url) {
    $.post(url, {},
		function (data, textStatus) {
		    if (data.Result) {
		        alert('上班签到成功！请不要忘了下班签到！');
		    }
		    else {
		        alert('上班签到失败！每天只能签到一次！');
		    }
		}, "json");
}

function CheckOut(url) {
    $.post(url, {},
		function (data, textStatus) {
		    if (data.Result) {
		        alert('下班签到成功！如果继续加班可以再次点击下班签到！');
		    }
		    else {
		        alert('签到失败！是否忘了上班签到？');
		    }
		}, "json");
}

function reflash(item) {
    $(item).parent().parent().remove();
    return true;
}