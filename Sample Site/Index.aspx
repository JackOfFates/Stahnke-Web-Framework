<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Index.aspx.vb" Inherits="Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index</title>
    <script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="js/QueryBuilder.js" type="text/javascript"></script>
    <script src="js/Main.js" type="text/javascript"></script>
    <script src="js/Controllers/Seller.js" type="text/javascript"></script>
    <script src="js/Controllers/User.js" type="text/javascript"></script>
    <script>
        var User = new UserAuth();
        var Seller = new SellerAuth();
        $(document).ready(function () {
            try {
                $('#seller').click(function () {
                    if (this.checked) {
                        $('#sellerDiv').css('display', 'initial');
                    } else {
                        $('#sellerDiv').css('display', 'none');
                    }
                });
            } catch (e) {
            }
        })

        function SetLoginStatus(success, data) {
            if (success) {
                $('#result').css('color', 'green');
                $('#result').html('Success! Logging in..');
                setTimeout(function () { location.reload(); }, 1000);
            } else {
                $('#result').css('color', 'red');
                $('#result').html('Login Failed!<br />' + data);
            }
        }

        function SetRegisterStatus(success, data) {
            if (success) {
                $('#rresult').css('color', 'green');
            } else {
                $('#rresult').css('color', 'red');
            }
            $('#rresult').text(data);
        }

        function doUpdateStandard() {
            if (RegisterUpdatePwCheck()) {
                User.Update($('#rpassword').val(),
                                    $('#firstname').val(),
                                    $('#lastname').val(),
                                    $('#email').val(),
                                    $('#bio').val(),
                                    $('#contact').val(),
                                    $('#seller')[0].checked,
                                    function (success, data) { SetRegisterStatus(success, data); });
            } else {
                SetRegisterStatus(false, 'Passwords do not match.');
            }
        }

        function doUpdateSeller() {
            if (RegisterUpdatePwCheck()) {
                User.Update($('#rpassword').val(),
                                    $('#firstname').val(),
                                    $('#lastname').val(),
                                    $('#email').val(),
                                    $('#bio').val(),
                                    $('#seller')[0].checked,
                                    $('#contact').val(),
                                    function (success, data) { SetRegisterStatus(success, data); });
                Seller.Update($('#businessname').val(),
                              $('#businesswebsite').val(),
                              $('#businessbiography').val(),
                              $('#businesslocation').val(),
                               function (success, data) { SetRegisterStatus(success, data); });
            } else {
                SetRegisterStatus(false, 'Passwords do not match.');
            }
        }

        function RegisterUpdatePwCheck() {
            return $('#rpassword').val() == $('#rcpassword').val();
        }

        function doRegister() {
            if (RegisterUpdatePwCheck()) {
                if ($('#seller')[0].checked) {
                    User.RegisterSeller($('#rusername').val(),
                                        $('#rpassword').val(),
                                        $('#firstname').val(),
                                        $('#lastname').val(),
                                        $('#email').val(),
                                        $('#bio').val(),
                                        $('#contact').val(),
                                        $('#seller')[0].checked,
                                        $('#businessname').val(),
                                        $('#businesswebsite').val(),
                                        $('#businessbiography').val(),
                                        $('#businesslocation').val(),
                                        function (success, data) { SetRegisterStatus(success, data); });

                } else {
                    User.RegisterStandard($('#rusername').val(),
                                          $('#rpassword').val(),
                                          $('#firstname').val(),
                                          $('#lastname').val(),
                                          $('#email').val(),
                                          $('#bio').val(),
                                          $('#contact').val(),
                                          function (success, data) { SetRegisterStatus(success, data); });
                }
            } else {
                SetRegisterStatus(false, 'Passwords do not match.');
            }
        }

    </script>
    <style>
        #sellerDiv {
            display: none;
        }
    </style>
</head>
<body>
    <%  Dim S As Database.session = Context.SqlSession
        Dim A As Database.account = Context.Account
        Dim isSeller As Boolean = If(A IsNot Nothing, A.AccountType = "SELLER", False)
    %>
    <%If S IsNot Nothing OrElse S.Username = Nothing Then %>
    <h2>Logged In As '<%=S.Username%>'</h2>
    <input type="button" value="Logout" onclick="User.Logout();" /><br />
    <br />
    <label style="color: green" id="result">Expires: <%=S.Expires.ToString%></label><br />

    <br />
    <br />

    <h2>Update Account</h2>
    <label>Password</label><br />
    <input type="password" id="rpassword" /><br />
    <label>Confirm Password</label><br />
    <input type="password" id="rcpassword" /><br />
    <label>First Name</label><br />
    <input type="text" id="firstname" value="<%=A.FirstName %>" /><br />
    <label>Last Name (Optional)</label><br />
    <input type="text" id="lastname" value="<%=A.LastName %>" /><br />
    <label>Email</label><br />
    <input type="text" id="email" value="<%=A.Email %>" /><br />
    <label>Biography</label><br />
    <textarea id="bio"><%=A.Biography%></textarea><br />
    <label>Public Contact Information</label><br />
    <textarea id="contact"></textarea><br />
    <label>Seller Account?</label><br />
    <input type="checkbox" id="seller" <%=If(A.AccountType = "SELLER", "checked=""Checked""", "")%>/><br />
    <%If isSeller %>
    <%Dim Seller As Database.Seller = Context.Seller %>
    <label>Business Name</label><br />
    <input type="text" id="businessname" value="<%=Seller.businessname %>" /><br />
    <label>Website (Optional)</label><br />
    <input type="text" id="businesswebsite" value="<%=Seller.businesswebsite %>" /><br />
    <label>Biography</label><br />
    <textarea id="businessbiography"><%=Seller.businessbiography %></textarea><br />
    <label>Location</label><br />
    <textarea id="businesslocation"><%=Seller.businesslocation %></textarea><br />
    <%End if %>
    <input type="submit" onclick="doUpdateSeller();" value="update" /><br />
    <label id="rresult"></label>
    <% Else %>
    <h2>Login Test</h2>
    <br />
    <label>Username</label><br />
    <input type="text" id="username" /><br />
    <label>Password</label><br />
    <input type="password" id="password" /><br />
    <input type="submit" onclick="User.Login($('#username').val(), $('#password').val(), function (success, data) { SetLoginStatus(success, data); });" value="login" /><br />
    <label id="result"></label>
    <br />
    <br />
    <h2>Register Test</h2>
    <label>Username</label><br />
    <input type="text" id="rusername" /><br />
    <label>Password</label><br />
    <input type="password" id="rpassword" /><br />
    <label>Confirm Password</label><br />
    <input type="password" id="rcpassword" /><br />
    <label>First Name</label><br />
    <input type="text" id="firstname" /><br />
    <label>Last Name (Optional)</label><br />
    <input type="text" id="lastname" /><br />
    <label>Email</label><br />
    <input type="text" id="email" /><br />
    <label>Biography</label><br />
    <textarea id="bio"></textarea><br />
    <label>Public Contact Information</label><br />
    <textarea id="contact"></textarea><br />
    <label>Seller Account?</label><br />
    <input type="checkbox" id="seller" /><br />
    <div id="sellerDiv">
        <label>Business Name</label><br />
        <input type="text" id="businessname" /><br />
        <label>Website (Optional)</label><br />
        <input type="text" id="businesswebsite" /><br />
        <label>Biography</label><br />
        <textarea id="businessbiography"></textarea><br />
        <label>Location</label><br />
        <textarea id="businesslocation"></textarea><br />
    </div>
    <input type="submit" onclick="doRegister();" value="register" /><br />
    <label id="rresult"></label>
    <% End If %>
</body>
</html>
