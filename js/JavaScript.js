$(function () {
    var chatHub = $.connection.chatHub;
     registerClientMethods(chatHub);
    // Start Hub
    $.connection.hub.start().done(function () {

        console.log("connection succeded");
        registerEvents(chatHub);
       

    });
    //chatHub.client.messageReceived = function (userName, message, time, userimg) {

    //    AddMessage(userName, message, time, userimg);
    //    console.log("message Received");
        // Display Message Count and Notification
        //var CurrUser1 = $('#hdUserName').val();
        //if (CurrUser1 != userName) {

        //    var msgcount = $('#MsgCountMain').html();
        //    msgcount++;
        //    $("#MsgCountMain").html(msgcount);
        //    $("#MsgCountMain").attr("title", msgcount + ' New Messages');
        //    var Notification = 'New Message From ' + userName;
        //    IntervalVal = setInterval("ShowTitleAlert('SignalR Chat App', '" + Notification + "')", 800);

        //}
   // }

    //$('#sendbutton1').click(function () {

    //    var msg = $('#txtmsg').val();

    //    if (true) {

    //        // var userName = $('#hdUserName').val();
    //        var userName = "pushkar";
    //        //var date = GetCurrentDateTime(new Date());
    //        var date = "date";

    //        chatHub.server.sendMessageToAll(userName, msg, date);
    //        $("#txtmsg1").val('');
    //    }
 
  
});

//function registerEvents(chatHub) {

//    var name = '<%# this.UserName %>';
//    var badge = '<%# this.UserBadge %>';
//    var enrollno = '<%# this.UserEnrollNo %>';
//    var department = '<%# this.UserDepartment %>';
//    var email = '<%# this.UserEmail %>';


//    if (name.length > 0) {
//        chatHub.server.connect(name, badge, enrollno, department, email);

//    }


//    // Clear Chat
//    //$('#btnClearChat').click(function () {

//    //    var msg = $("#divChatWindow").html();

//    //    if (msg.length > 0) {
//    //        chatHub.server.clearTimeout();
//    //        $('#divChatWindow').html('');

//    //    }
//    //});

//    // Send Button Click Event
//    $('#sendbutton1').click(function () {

//        var msg = $("#txtmsg1").val();
//        //check length of message code is to be added
//        if (true) {

//          //  var userName = $('#hdUserName').val();
//            var userName = "Admin";
//            //var date = GetCurrentDateTime(new Date());
//            var date = "date";
//            console.log("send message to all called");

//            chatHub.server.sendMessageToAll(userName, msg, date);
//            $("#txtmsg1").val('');
//        }
//    });

//    // Send Message on Enter Button
//    $("#txtMessage").keypress(function (e) {
//        if (e.which == 13) {
//            $('#sendbutton1').click();
//        }
//    });

   

//}
//function registerClientMethods(chatHub) {


   
  
//}

//function GetCurrentDateTime(now) {

//    var localdate = dateFormat(now, "dddd, mmmm dS, yyyy, h:MM:ss TT");

//    return localdate;
//}

function AddMessage(userName, message, time, userimg) {
    console.log("Add Message Called");

    var CurrUser = $('#hdUserName').val();
    var Side = 'left';
    var TimeSide = 'right';

    //if (CurrUser == userName) {
    //    Side = 'left';
    //    TimeSide = 'right';

    //}
   
   // var divChat = '<div class="bg-primary p-2 mt-2 mr-5 shadow-sm text-white float-right rounded ">'+message+'</div>';


    var divChat = '<div class="direct-chat-msg ' + Side + '">' +
        '<div class="direct-chat-info clearfix">' +
       // '<span class="direct-chat-name pull-' + Side + '">' + userName + '</span>' +
        '<span class="direct-chat-timestamp pull-' + TimeSide + '"">' + time + '</span>' +
        '</div>' +

       // ' <img class="direct-chat-img" src="' + userimg + '" alt="Message User Image">' +
        ' <div class="direct-chat-text" >' + message + '</div> </div>';
    $('#msgarea').append(divChat);      
    
    var height = $('#msgarea')[0].scrollHeight;

    // Apply Slim Scroll Bar in Group Chat Box
    $('#msgarea').slimScroll({
        height: height
    });

    //ParseEmoji('#divChatWindow');
    console.log("suuuuuuuuuccccesss");

};
function registerClientMethods(chatHub) {

    // Calls when user successfully logged in
    chatHub.client.onConnected = function (id, userName, allUsers, messages, times) {
        console.log("onConnected Called");

        $('#hdId').val(id);
        $('#hdUserName').val(userName);
        $('#spanUser').html(userName);

        // Add All Users
        for (i = 0; i < allUsers.length; i++) {

           // AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName, allUsers[i].UserImage, allUsers[i].LoginTime);
        }

        // Add Existing Messages
        for (i = 0; i < messages.length; i++) {
           // AddMessage(messages[i].UserName, messages[i].Message, messages[i].Time, messages[i].UserImage);

        }
    }
    chatHub.client.loadRegisteredUsers = function (users) {
        var i;
        var add;
        var add1;
        var add2;
     
        for (i = 0; i < users.length; i++)
        {
            add = ' <tr id="'+users[i][1] +'" }> <td><img src="images/p2.jpg" alt="" class="profile-image rounded-circle" /></td>';
               
            add1 = '<td>' + users[i][0] + ' <br /> <small>achi chal rahi</small></td>';

            add2 = '<td><small>11:55 PM</small></td></tr>';
            $('#registered_users').append(add + add1 + add2);        
        }

    }
    // On New User Connected
    chatHub.client.onNewUserConnected = function (id, name, UserImage, loginDate) {
        AddUser(chatHub, id, name, UserImage, loginDate);
    }

    // On User Disconnected
    chatHub.client.onUserDisconnected = function (id, userName) {

        $('#Div' + id).remove();

        var ctrId = 'private_' + id;
        $('#' + ctrId).remove();


        var disc = $('<div class="disconnect">"' + userName + '" logged off.</div>');

        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);

    }

    chatHub.client.messageReceived = function (userName, message, time, userimg) {

        AddMessage(userName, message, time, userimg);

        // Display Message Count and Notification
        var CurrUser1 = $('#hdUserName').val();
        if (CurrUser1 != userName) {

            var msgcount = $('#MsgCountMain').html();
            msgcount++;
            $("#MsgCountMain").html(msgcount);
            $("#MsgCountMain").attr("title", msgcount + ' New Messages');
            var Notification = 'New Message From ' + userName;
            IntervalVal = setInterval("ShowTitleAlert('SignalR Chat App', '" + Notification + "')", 800);

        }
    }


    chatHub.client.sendPrivateMessage = function (windowId, fromUserName, message, userimg, CurrentDateTime) {

        var ctrId = 'private_' + windowId;
        if ($('#' + ctrId).length == 0) {

            OpenPrivateChatBox(chatHub, windowId, ctrId, fromUserName, userimg);

        }

        var CurrUser = $('#hdUserName').val();
        var Side = 'right';
        var TimeSide = 'left';

        if (CurrUser == fromUserName) {
            Side = 'left';
            TimeSide = 'right';

        }
        else {
            var Notification = 'New Message From ' + fromUserName;
            IntervalVal = setInterval("ShowTitleAlert('SignalR Chat App', '" + Notification + "')", 800);

            var msgcount = $('#' + ctrId).find('#MsgCountP').html();
            msgcount++;
            $('#' + ctrId).find('#MsgCountP').html(msgcount);
            $('#' + ctrId).find('#MsgCountP').attr("title", msgcount + ' New Messages');
        }

        var divChatP = '<div class="direct-chat-msg ' + Side + '">' +
            '<div class="direct-chat-info clearfix">' +
            '<span class="direct-chat-name pull-' + Side + '">' + fromUserName + '</span>' +
            '<span class="direct-chat-timestamp pull-' + TimeSide + '"">' + CurrentDateTime + '</span>' +
            '</div>' +

            ' <img class="direct-chat-img" src="' + userimg + '" alt="Message User Image">' +
            ' <div class="direct-chat-text" >' + message + '</div> </div>';

        $('#' + ctrId).find('#divMessage').append(divChatP);

        // Apply Slim Scroll Bar in Private Chat Box
        var ScrollHeight = $('#' + ctrId).find('#divMessage')[0].scrollHeight;
        $('#' + ctrId).find('#divMessage').slimScroll({
            height: ScrollHeight
        });
    }


//};
//function registerEvents(chatHub) {

//    var name = '<%# this.UserName %>';
//    var badge = '<%# this.UserBadge %>';
//    var enrollno = '<%# this.UserEnrollNo %>';
//    var department = '<%# this.UserDepartment %>';
//    var email = '<%# this.UserEmail %>';


//    if (name.length > 0) {
//        chatHub.server.connect(name, badge, enrollno, department, email);

//    }


//    // Clear Chat
//    $('#btnClearChat').click(function () {

//        var msg = $("#divChatWindow").html();

//        if (msg.length > 0) {
//            chatHub.server.clearTimeout();
//            $('#divChatWindow').html('');

//        }
//    });

//    // Send Button Click Event
//    $('#btnSendMsg').click(function () {

//        var msg = $("#txtMessage").val();

//        if (msg.length > 0) {

//            var userName = $('#hdUserName').val();

//           // var date = GetCurrentDateTime(new Date());
//            var date = 'date';
//            chatHub.server.sendMessageToAll(userName, msg, date);
//            $("#txtMessage").val('');
//        }
//    });

//    // Send Message on Enter Button
//    $("#txtMessage").keypress(function (e) {
//        if (e.which == 13) {
//            $('#btnSendMsg').click();
//        }
//    });


};