<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="chat.ascx.cs" Inherits="Posy.V2.WF.P.chat" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>

<div class="cd-cart-container">
    <a href="#0" class="cd-cart-trigger">Cart
	
        <ul class="count">
            <!-- cart items count -->
            <li>0</li>
            <li>0</li>
        </ul>
        <!-- .count -->
    </a>

    <div class="cd-cart cnt-chat">
        <div class="wrapper">
            <div class="body">


<!-- CHAT -->

<!--div class="cnt-chat"-->

    <div class="chatroom">

        <div class="menu">
            <a href="#" class="back"><i class="fa fa-angle-left"></i>
                <img src="<%= Funcao.ResolveServerUrl("~/img/perfil/0.jpg", false) %>" draggable="false" />
            </a>
            <div class="name">Chat</div>
            <!--div class="members">Sereia</div-->
        </div>

        <ol class="chat" id="chatWindow">
            <!--li class="other">
            <div class="msg">
                <div class="user">Poseydon<span class="range admin">Admin</span></div>
                <p>Dude</p>
                <p>
                    Want to go dinner?
                        <emoji class="pizza"></emoji>
                </p>
                <time>20:17</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Sereia</div>
                <p>
                    I'm still doing the Góngora comment...
                        <emoji class="books"></emoji>
                </p>
                <p>Better other day</p>
                <time>20:18</time>
            </div>
        </li>
        <li class="self">
            <div class="msg">
                <p>
                    I'm still doing the Góngora comment...
                        <emoji class="books"></emoji>
                </p>
                <p>Better other day</p>
                <time>20:18</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Brotons</div>
                <p>
                    What comment about Góngora?
                        <emoji class="suffocated"></emoji>
                </p>
                <time>20:18</time>
            </div>
        </li>
        <li class="self">
            <div class="msg">
                <p>The comment sent Marialu</p>
                <p>It's for tomorrow</p>
                <time>20:18</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Brotons</div>
                <p>
                    <emoji class="scream"></emoji>
                </p>
                <p>
                    Hand it to me!
                        <emoji class="please"></emoji>
                </p>
                <time>20:18</time>
            </div>
        </li>
        <li class="self">
            <div class="msg">
                <img src="http://i.imgur.com/kUPxcsI.jpg" draggable="false" />
                <time>20:19</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Brotons</div>
                <p>
                    Thank you!
                        <emoji class="hearth_blue"></emoji>
                </p>
                <time>20:20</time>
            </div>
        </li>
        <div class="day">Today</div>
        <li class="self">
            <div class="msg">
                <p>Who wants to play Minecraft?</p>
                <time>18:03</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Charo</div>
                <p>Come on, I didn't play it for four months</p>
                <time>18:07</time>
            </div>
        </li>
        <li class="self">
            <div class="msg">
                <p>
                    Ehh, the launcher crash...
                        <emoji class="cryalot"></emoji>
                </p>
                <time>18:08</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Charo</div>
                <p>
                    <emoji class="lmao"></emoji>
                </p>
                <p>Sure that is the base code</p>
                <p>I told it to Mojang</p>
                <time>18:08</time>
            </div>
        </li>
        <li class="self">
            <div class="msg">
                <p>It's a joke</p>
                <p>Moai attack!</p>
                <p>
                    <emoji class="moai"></emoji>
                    <emoji class="moai"></emoji>
                    <emoji class="moai"></emoji>
                    <emoji class="moai"></emoji>
                    <emoji class="moai"></emoji>
                    <emoji class="moai"></emoji>
                </p>
                <time>18:10</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Charo</div>
                <p>XD</p>
                <p>
                    <emoji class="funny"></emoji>
                </p>
                <p>Heart for this awesome design!</p>
                <time>18:08</time>
            </div>
        </li>
        <p class="notification">David joined the group <time>18:09</time></p>
        <li class="self">
            <div class="msg">
                <p>
                    Heeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeellooooooooooooooooooooooooooooooo David
                        <emoji class="smile" />
                </p>
                <time>18:09</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">David</div>
                <p>
                    What is that
                        <emoji class="shit"></emoji>
                    ?
                </p>
                <time>18:10</time>
            </div>
        </li>
        <p class="notification">David left the group <time>18:11</time></p>
        <li class="other">
            <div class="msg">
                <div class="user">Brotons</div>
                <p>Lol?</p>
                <time>18:12</time>
            </div>
        </li>
        <li class="other">
            <div class="msg">
                <div class="user">Marga<span class="range admin">Admin</span></div>
                <p>I'm boring...</p>
                <p>
                    Who wants to do some logarithms?
                        <emoji class="smile"></emoji>
                </p>
                <time>18:15</time>
            </div>
        </li-->
        </ol>
        <div class="typezone">
            <input type="text" class="textarea" placeholder="Mensagem" id="mensagem" /><input type="button" class="send" value="" />
            <div class="emojis"></div>
        </div>

    </div>

    <div class="chatmembers">

        <!--div class="mb-attribution animated slideInUp" cp="eda13159-a093-4c82-9283-f1ddfff69e2d">
            <p class="mb-author">Era Terra</p>
            <cite>Jane Eyre</cite>
            <div class="mb-thumb" style="background-image: url(img/perfil/eda13159-a093-4c82-9283-f1ddfff69e2d/1.jpg);"></div>
            <div class="mb-notification"><span></span>12:00</div>
        </div>
        <div class="mb-attribution animated slideInUp" cp="bc0083ae-9b49-471c-9ab1-61bd763e8b9b">
            <p class="mb-author">Poseydon Espilacopa</p>
            <cite>Jane Eyre</cite>
            <div class="mb-thumb" style="background-image: url(img/perfil/bc0083ae-9b49-471c-9ab1-61bd763e8b9b/1.jpg);"></div>
            <div class="mb-notification"><span></span>12:00</div>
        </div>

        <div class="mb-attribution animated slideInUp" cp="dabc9623-2bf3-463d-b665-8dd7ce583970">
            <p class="mb-author">Sereia Élphis</p>
            <cite>Jane Eyre</cite>
            <div class="mb-thumb" style="background-image: url(img/perfil/dabc9623-2bf3-463d-b665-8dd7ce583970/1.jpg);"></div>
            <div class="mb-notification"><span></span>12:00</div>
        </div>
        <div class="mb-attribution animated slideInUp" cp="27e2ecb0-9674-4ff3-ae65-36a07764ed21">
            <p class="mb-author">Anfitrite Espilacopa</p>
            <cite>Jane Eyre</cite>
            <div class="mb-thumb" style="background-image: url(img/perfil/27e2ecb0-9674-4ff3-ae65-36a07764ed21/1.jpg);"></div>
            <div class="mb-notification"><span></span>12:00</div>
        </div-->

    </div>

<!--/div-->

<!-- CHAT -->


            </div>
        </div>
    </div>
    <!-- .cd-cart -->
</div>
<!-- cd-cart-container -->




<script src="<%= Funcao.ResolveServerUrl("~/js-dev/chat.js", false) %>" type="text/javascript"></script>
