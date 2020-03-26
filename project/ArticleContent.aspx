<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleContent.aspx.cs" Inherits="project_ArticleContent" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content="請填寫"/>
    <meta name="keywords" content="請填寫"/>
    <meta name="author" content="請填寫"/>

    <!--=====Favicon-->
    <link rel="shortcut icon" href="favicon.ico" />
    <!--=====CSS Global Compulsory -->
    <link rel="stylesheet" href="../assets/plugins/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/style.css" />
    <!--======CSS Implementing Plugins -->
    <link rel="stylesheet" href="../assets/plugins/font-awesome/css/font-awesome.min.css" />
    <!--icon的元件-->
    <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css" />
    <!--=====CSS Customization -->
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <!--=====CSS Customization by ochison  -->
    <link rel="stylesheet" href="../assets/css/iekicon.css" />
    <link rel="stylesheet" href="../assets/css/scrollbar.css" />
    <link rel="stylesheet" href="../assets/css/ochi.css" />

    <!-- JS Global Compulsory -->
    <script type="text/javascript" src="../assets/plugins/jquery/jquery.js"></script>
    <script type="text/javascript" src="../assets/plugins/jquery/jquery-migrate.min.js"></script>
    <!-- JS Implementing Plugins -->
    <script type="text/javascript" src="../assets/plugins/back-to-top.js"></script>
    <!-- JS Customization -->
    <script type="text/javascript" src="../assets/js/custom.js"></script>
    <!-- JS Page Level -->
    <script type="text/javascript" src="../assets/js/app.js"></script>
    <!--看圖-->
    <script type="text/javascript" src="../assets/js/jquery.scrollbar.min.js"></script>
    <script type="text/javascript" src="../assets/js/ochiJS.js"></script>

    <!--[if lt IE 9]>
        <script src="../assets/plugins/respond.js"></script>
        <script src="../assets/plugins/html5shiv.js"></script>
        <script src="../assets/js/plugins/placeholder-IE-fixes.js"></script>
        <![endif]-->

    <!--===my d3-->
    <script src="../js/textCloud/d3.js"></script>
    <script src="../js/textCloud/d3.layout.cloud.js"></script>

    <!-- Nick.JS -->
    <script type="text/javascript" src="../js/NickCommon.js"></script>
    <script type="text/javascript" src="../js/PageList.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.1.12.1.js"></script>
    <script>$.widget.bridge('uitooltip', $.ui.tooltip);</script> <!--JQueryUI & bootstrap tooltip 會打架 重新設定 JQueryUI tooltip function name-->
    <!-- bootstrap -->
    <script type="text/javascript" src="../assets/plugins/bootstrap/js/bootstrap.min.js"></script>

    <!--===my js-->
    <script type="text/javascript" src="articleContent.js"></script>
    <script type="text/javascript" src="wordcloud.js"></script>
    <title>IEKElf</title>
</head>
<body class="header-fixed boxed-layout">
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
          <div class="container">
              <!--文字雲-->
              <div class="twocol margin10T"><div class="left"><H3>See First : Word Cloud</H3></div></div>
              <div class="maxheightB BoxBorderSa BoxBgWa padding5ALL"><div id="blockTag" class="width100"></div></div>
                 
                <div class="twocol">
                    <div class="left"><h3>Summary</h3></div>
                </div>

                <div class="BoxBgWa margin-bottom-20">
                    <div style="font-size:14pt; margin-bottom:10px;"><b>Auto Summary</b></div>
                    <div id="Summary">
                        The cell phone industry looks to bounce back at mwc 2019 with the smart phone industry struggling as of late, it can be expected that mwc 2019 will produce all sorts of oddities to tempt buyers. The event theme was `` intelligent connectivity. ''my subjective takeaways from mwc19 are : also : tech companies are dialling down the hype around 5g mobile5g, ai, and iot form a new trinity. Mwc 2019 why hololens 2 is a cloud play huawei 's 5g foldable 5g has one big design difference lg unveils 5g smartphone v50 thinq nokia ceo : we have the right 5g strategy hmd phones include nokia 9 pureview with 5 cameras iot : lenovo 's edge server is n't much bigger than a notebook cnet : latest from barcelona windows 10 pcs to get 5g for first time as qualcomm unveils new modem last week mobile world congress ( mwc ) took place in barcelona. Rather than discussing 5g as a standalone technology, as in 2018, the 5g debate has matured in 2019. This year, 5g was discussed in combination with ai and iot. At the same time, the event was overshadowed by geopolitical squabbles over network security as well as greater scrutiny of financial viability regarding emerging technology investments such as 5g, the dominating theme at the event. Several supposedly 5g-capable handsets were announced ( e.g., lg v50 thinq, huawei mate x, and samsung galaxy s10 5g ), 5g base stations were shown, and intelligent network management solutions ( e.g., telefonica 's kite iot platform ) were demonstrated. This theme captured well what is happening in the market. At mwc19, the ai hype evaporated further than at mwc18 and morphed from a sexy story into an important tool to manage networks more efficiently and autonomously via machine learning. Iot was primarily discussed at mwc19 from a `` connected sensor '' perspective. 
                    </div>
                </div>

               <div id="ArticleTitle"><h3>Toward A Quantum Internet</h3></div>
               <div id="WebSite">Article from: <a href="javascript:void(0);">mit</a></div>

              <div id="ArticleContent" class="BoxBgWa margin-bottom-20">
                  The promise of quantum computers is tantalizingly great: near-instantaneous problem solving, and perfectly secure data transmission. For the most part, however, small-scale demonstrations of quantum computation remain isolated in labs throughout the world. Now, Prem Kumar, a professor of electrical engineering and computer science at Northwestern University, has taken a step toward making quantum computing more practical. Kumar and his team have shown that they can build a quantum logic gate–a fundamental component of a quantum computer–within an optical fiber. The gate could be part of a circuit that relays information securely, over hundreds of kilometers of fiber, from one quantum computer to another. It could also be used on its own to find solutions to complicated mathematical problems.  		   	     	  	 		  		 			     		  		  	  	 		 			 			Entangled Web: The optical components on this lab bench, such as mirrors and filters, allow researchers in Prem Kumar’s lab at Northwestern University to direct and manipulate light. In Kumar’s most recent work, he has created a quantum logic gate within an optical fiber; such gates could eventually enable networks of quantum computers. 			 			 		 		     		A logic gate is a device that receives an input, performs a logic operation on it, and produces an output. The type of gate that Kumar created, called a controlled NOT gate, has a classical-computing analogue that flips a bit registering a “1” to “0,” and vice versa. Quantum logic gates like Kumar’s have been built before, but they worked with laser beams that passed through the air, not through fiber. The new gate lays the foundation for experiments that demonstrate the abilities of quantum computers in fiber, says Kumar. “The exciting thing here is that an application is within reach,” he says. Within the next year, Kumar and his team plan to test the gate in a specific application: conducting a complex auction over a secure quantum network.   		Researchers at IBM, MIT, and many other corporations and universities have been working on quantum computers since they were first proposed in the 1980s. A quantum computer is a device that processes bits of information by exploiting the weird quantum-mechanical properties of particles such as electrons and photons. A quantum computer is theoretically able to process exponentially more information than classical computers can. The unit of information in a classical computer is the bit, which represents either a “1” or a “0”; but in a quantum computer, it’s the qubit, which can represent both a “1” and a “0” at the same time. Since qubits compute with multiple values at once, the processing power of a quantum computer doubles with each additional qubit. This characteristic would enable a quantum computer with only a couple hundred qubits to significantly outperform today’s best supercomputers.   		Kumar’s group makes qubits out of photons that are “entangled.” That means that their physical characteristics, such as polarization, are linked in such a way that if one photon assumes a particular physical state, the matching photon instantly assumes a corresponding state. A few years ago, Kumar demonstrated that optical fiber itself could cause photons to become entangled, and that they would remain entangled over a distance of 100 kilometers. His recent work, described in Physical Review Letters, goes one step further, creating a logic gate that entangles photon pairs.  		  To use this gate, Kumar needs photons that are identical in every way except polarization, or the orientation of their electromagnetic fields.These “identical” photons are sent through optical fiber to the gate itself, a small maze of devices that route photons in different directions depending on their polarization. Passing through the maze causes certain photon pairs to become entangled. But not all photons make it through the gate; only when photons reach detectors on the other end, and the researchers can measure whether or not they are entangled, do they know the gate succeeded.  		The only way to know whether or not the gate worked is to wait until a collection of photons has been fired at it, says Carl Williams, coordinator of the quantum information program at the National Institute of Standards and Technology. “Most of the time the gate fails,” he says. “It’s a probabilistic thing.” But when the gate fails, the researchers simply disregard the unentangled photons.  		“The great thing about this work,” says Williams, “is that it’s in fiber. This is a big deal because it could lead to distributed networks. … The obvious application is for long-distance quantum communication between two smaller quantum computers.” One of the crucial elements in a conventional optical network is a device called a repeater, which amplifies signals that have degraded over distance. Williams says that a quantum logic gate, such as the one that Kumar built, could be used in a circuit that amplifies a signal without losing the entanglement of the photons.  		“This is an important step toward constructing a quantum Internet,” says Seth Lloyd, a professor of mechanical engineering at MIT and a leading researcher in quantum computation. “Such a network would have powers that the ordinary Internet does not,” he says. “In particular, communication over the quantum Internet would be automatically secure.”  		Lloyd notes that Kumar’s paper illustrates how a simple quantum logic operation can be performed using individual photons. “The current paper represents a significant advance in the technology of quantum computation and quantum networks,” he says. 	 		 			AI is here.
Own what happens next at EmTech Digital 2019. 			 			Register now
              </div>
          </div>
         <!--#include file="../templates/Footer.html"-->
    </div>
</body>
</html>
