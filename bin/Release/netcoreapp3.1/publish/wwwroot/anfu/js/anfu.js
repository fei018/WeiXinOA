function SubmitLoading() {
    var $ = layui.$;
    $('.submit_loading').css({ 'display': 'block' });
}

function UnSubmitLoding() {
    var $ = layui.$;
    $('.submit_loading').css({ 'display': 'none' });
}  

function LayoutAdmin() {
    var element = layui.element;
    var $ = layui.$;

    //手机设备的简单适配
    $('.site-tree-mobile').on('click', function () {
        $('body').addClass('site-mobile');
    });

    $('.site-mobile-shade').on('click', function () {
        $('body').removeClass('site-mobile');
    });
}

function ApplyFormElder() {
    var form = layui.form;
    var laydate = layui.laydate;
    var element = layui.element;
    var $ = layui.jquery;
    var layer = layui.layer;

    laydate.render({
        elem: '#date1'
    });

    laydate.render({
        elem: '#date2'
    });
}

function ApplyFormVolunteer() {
    var form = layui.form;
    var laydate = layui.laydate;
    var element = layui.element;
    var $ = layui.jquery;
    var layer = layui.layer;

    laydate.render({
        elem: '#date1'
    });
}

//检查图片大小不超过4M
function CheckSizeMax4M(input) {
    var layer = layui.layer;

    let size = input.files[0].size;
    size = size / 1024; //KB
    if (size > 4096) {
        input.value = '';
        layer.alert("图片大小不能超过4M");
    }
}

//Admin/ApplyForm/Elder
function AdminElderTable() {
    var table = layui.table;
    //var $ = layui.jquery;

    var tableIns = table.render({
        elem: '#table1'
        , url: '/Admin/ApplyForm/Elder/QueryAllPaged'
        , cellMinWidth: 100
        , page: {
            layout: ['prev', 'page', 'next', 'count', 'limit']
            , groups: 3
        }
        , request: { pageName: 'pages' } /*页码的参数名称，默认：page*/
        , cols: [[
            { field: 'name', title: '姓名' }
            , { field: 'sex', title: '性别' }
            , { field: 'age', title: '年龄' }
            , { field: 'phone', title: '电话' }
            , { field: 'addTime', title: '填表时间' }
            , { fixed: 'right', title: '', toolbar: '#caoZuoTpl' }
        ]]
    });
    return tableIns;
}

//function TryDownloadPhoto(btn,url) {
//    var $ = layui.jquery;
//    var cid = btn.value;
//    $.ajax({
//        type: "post",
//        async: false,
//        url: url,
//        data: { checkId: cid },
//        dataType: "json",
//        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
//        success: function (result) {
//            var code = result.code;
//            if (code === 400) {
//                alert(result.msg);
//            } else {
//                //下载
//                DowloadPhoto(btn);
//            }
//        }
//    });
//}

function DowloadPhoto(btn,url) {
    var $ = layui.jquery;
    $("#downloadId").val(btn.value);
    var $form = $("#downloadForm").attr("action", url);
    $form.submit();
}

function DownloadDocx(btn,url) {
    var $ = layui.jquery;
    $("#downloadId").val(btn.value);
    var $form = $("#downloadForm").attr("action", url);
    $form.submit();
}

//Admin/ApplyForm/volunteer
function AdminVolunteerTable() {
    var table = layui.table;
    //var $ = layui.jquery;
    //var layer = layui.layer;

    var tableIns = table.render({
        elem: '#table1'
        , url: '/Admin/ApplyForm/Volunteer/QueryAllPaged'
        , cellMinWidth: 100
        , page: {
            layout: ['prev', 'page', 'next', 'count', 'limit']
            , groups: 3
        }
        , request: { pageName: 'pages' } /*页码的参数名称，默认：page*/
        , cols: [[
            { field: 'name', title: '姓名', }
            , { field: 'sex', title: '性别' }
            , { field: 'age', title: '年龄' }
            , { field: 'phone', title: '电话' }
            , { field: 'addTime', title: '填表时间' }
            , { fixed: 'right', title: '下载', toolbar: '#caoZuoTpl' }
        ]]
    });

    return tableIns;
}