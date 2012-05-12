$(function () {
    SetNavClass('mainNav', 'active');
    SetNavClass('sideNav', 'active');
});

function SetNavClass(ulId, className) {
    var controller = $('#controller').val();
    var action = $('#action').val();
    eval('controller_' + controller + ' = true');
    eval('action_' + action + ' = true');
    list = $('#' + ulId + ' *');

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