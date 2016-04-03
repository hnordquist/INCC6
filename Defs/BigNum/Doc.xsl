<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<!--
Copyright Adam A. Brown, 2009
This XSLT stylesheet is the exclusive intellectual property of Adam Brown.
You may copy, modify and use this code for any purpose without prior authorization,
provided that you keep this attribution in the file, no matter what changes are made.
check www.fractal-landscapes.co.uk for any updates.
-->

<!-- Put this header as the second line of your XML file -->
<!-- <?xml-stylesheet type="text/xsl" href="Doc.xsl"?> -->

<!-- The main template -->
<xsl:template match="/">
<HTML>
	<!-- Simple CSS styles, inlined for ease of editing -->
	<style type="text/css">
		.indent { margin-left: 20px; }
		body{
			font-family: Verdana, Serif;
			font-size: 12px;
		}
		h1{
			font-size: 16px;
			font-weight: bold;
		}
		h2
		{
			font-size: 16px;
			font-weight: bold;
		}
		h3
		{
			font-size: 12px;
			font-weight: bold;
		}
		.cont
		{
			background-color: #efefef;
		}
		a.dsphead1{
   		text-decoration:none;
   		color: #5050bf;
   		margin-left:20px;}
   	a.dsphead1:hover{
   		color: #0000ff;
   	}
   	a.dsphead{
   		text-decoration:none;
   		color: #000000;
   		margin-left:20px;}
		a.dsphead:hover{
   		color: #0000ff;}
		a.dsphead span.dspchar{
   		color:normal;}
		.dspcont{
   		display:none;
   		color: #000000;
   		margin-left:30px;
   	}
   	.dspdisp{
   		color: #000000;
   		margin-left:30px;
   	}
   	.dspcode
   	{
   		margin-left: 10px;
   		border: 1px solid #000000;
   		width: 600px;
   		color: #000000;
   		background-color: #ffffff;
   		font-family: 'Courier New', Courier, monospace;
   		font-size: 12px;
   	}
   	.dspcomment
   	{
   		color: #007f00;
   	}
   	.paramtable{
   		width: 600px;
   		background-color: #dfdfdf;
   		border: 1px solid #000000;
   		margin-left:10px;
   	}
   	.paramheader{
   		background-color: #dfdfff;
   	}
   	.paramcell{
   		font-family: monospace;
   		font-size: 12px;
   		margin-left: 10px;
   		margin-right: 10px;
   	}
		</style>
		
		<script>
			function dsp(loc){
   if(document.getElementById){
      var foc=loc.firstChild;
      foc=loc.firstChild.innerHTML?
         loc.firstChild:
         loc.firstChild.nextSibling;
      foc.innerHTML=foc.innerHTML=='+'?'-':'+';
      foc=loc.parentNode.nextSibling.style?
         loc.parentNode.nextSibling:
         loc.parentNode.nextSibling.nextSibling;
      foc.style.display=foc.style.display=='block'?'none':'block';}}
	
		</script>
  <BODY>
  	<div class="cont">
    	<xsl:apply-templates select="//assembly"/>
    </div>
  </BODY>
</HTML>
</xsl:template>

<!-- Display a collapsible index of the items in the XML file, with an imposed hierarchy -->
<xsl:template match="assembly">
<H1><xsl:value-of select="name"/></H1>

	<h2>Assembly Index</h2>

	<p class="indent">
		
		<!-- Loop through the classes -->
		<xsl:for-each select="//member[contains(@name, 'T:')]">
			<xsl:variable name="_fullName" select="substring-after(@name, 'T:')"/>
			<h2>
				<a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)">
 				<span class="dspchar">+</span><xsl:value-of select="$_fullName"/><br/></a>
 			</h2>
 				<!-- Enumerate the members -->
 				<div class="dspcont">
 					<!-- Display the class summary -->
 					<xsl:apply-templates select="summary" />
 					
 					<!-- Display the constructor information for this class -->
 					<!-- The conditions here check that:
 						1. The function name specifies a constructor
 						2. The name of this type is at the beginning of the full function name
 						3. Nothing appears between the brackets and the (fullname + constructor) - i.e. not a subclass constructor
 						-->
 					<xsl:if test="count(//member[(contains(@name, '.#ctor') or contains(@name, '.#cctor')) and starts-with(substring-after(@name, 'M:'), $_fullName) and not(contains(substring-before(substring-after(@name, concat($_fullName, '.#ctor')), '('), '.'))]) != 0">	
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> Constructors</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[contains(@name, '.#cctor') and starts-with(substring-after(@name, 'M:'), $_fullName) and not(contains(substring-before(substring-after(@name, concat($_fullName, '.#cctor')), '('), '.'))]"/>
 						<xsl:apply-templates select="//member[contains(@name, '.#ctor') and starts-with(substring-after(@name, 'M:'), $_fullName) and not(contains(substring-before(substring-after(@name, concat($_fullName, '.#ctor')), '('), '.'))]"/></div>
 					</xsl:if>
 					
 					<!-- Display member functions -->
 					<xsl:if test="count(//member[not(contains(@name, '.#ctor')) and not(contains(@name, '.#cctor')) and not(contains(@name, '.op_')) and starts-with(substring-after(@name, 'M:'), $_fullName) and not(contains(substring-after(substring-before(@name, '('), concat($_fullName, '.')), '.'))]) != 0">	
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> Member Functions</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[not(contains(@name, '.#ctor')) and not(contains(@name, '.#cctor')) and not(contains(@name, '.op_')) and starts-with(substring-after(@name, 'M:'), $_fullName) and not(contains(substring-after(substring-before(@name, '('), concat($_fullName, '.')), '.'))]"/></div>
 					</xsl:if>
 					
 					<!-- Display implicit casts -->
 					<xsl:if test="count(//member[contains(@name, 'op_Implicit') and starts-with(substring-after(@name, 'M:'), $_fullName) and starts-with(substring-after(@name, $_fullName), '.op_Implicit')])">
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> Implicit casts</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[contains(@name, 'op_Implicit') and starts-with(substring-after(@name, 'M:'), $_fullName) and starts-with(substring-after(@name, $_fullName), '.op_Implicit')]"/></div>
 					</xsl:if>
 					
 					<!-- Display explicit casts -->
 					<xsl:if test="count(//member[contains(@name, 'op_Explicit') and starts-with(substring-after(@name, 'M:'), $_fullName) and starts-with(substring-after(@name, $_fullName), '.op_Explicit')])">
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> Explicit casts</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[contains(@name, 'op_Explicit') and starts-with(substring-after(@name, 'M:'), $_fullName) and starts-with(substring-after(@name, $_fullName), '.op_Explicit')]"/></div>
 					</xsl:if>
 					
 					<!-- Display operators -->
 					<xsl:if test="count(//member[contains(@name, '.op_') and not(contains(@name, '.op_Explicit')) and not(contains(@name, '.op_Implicit')) and starts-with(substring-after(@name, 'M:'), $_fullName) and starts-with(substring-after(@name, $_fullName), '.op_')])">
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> operators</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[contains(@name, '.op_') and not(contains(@name, '.op_Explicit')) and not(contains(@name, '.op_Implicit')) and starts-with(substring-after(@name, 'M:'), $_fullName) and starts-with(substring-after(@name, $_fullName), '.op_')]"/></div>
 					</xsl:if> 					
 					
 					<!-- Display the property information for this class -->
 					<xsl:if test="count(//member[contains(@name, 'P:') and starts-with(substring-after(@name, 'P:'), $_fullName) and not(contains(substring-after(@name, concat($_fullName, '.')), '.'))]) != 0">	
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> Properties</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[contains(@name, 'P:') and starts-with(substring-after(@name, 'P:'), $_fullName) and not(contains(substring-after(@name, concat($_fullName, '.')), '.'))]"/></div>
 					</xsl:if> 					
 					
 					<!-- Display the property information for this class -->
 					<xsl:if test="count(//member[contains(@name, 'F:') and starts-with(substring-after(@name, 'F:'), $_fullName) and not(contains(substring-after(@name, concat($_fullName, '.')), '.'))]) != 0">	
 						<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)"><span class="dspchar">+</span> Enum members</a></h3>
 						<div class="dspcont"><xsl:apply-templates select="//member[contains(@name, 'F:') and starts-with(substring-after(@name, 'F:'), $_fullName) and not(contains(substring-after(@name, concat($_fullName, '.')), '.'))]"/></div>
 					</xsl:if>
 					
 					
 				</div>

		</xsl:for-each>
	</p>
  
</xsl:template>

<!-- Member display -->
<xsl:template match="member[contains(@name, 'M:') and not(contains(@name, '.op_')) and not(contains(@name, '.#ctor'))]">
	<xsl:variable name="_fullName" select="substring-before(substring-after(@name, 'M:'), '(')"/>
	
		
		<xsl:choose>	
			<xsl:when test="string-length(normalize-space(substring-after(substring-before(@name, ')'), '('))) = 0">
				<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)"><span class="dspchar">+</span> 
					<xsl:call-template name="removeQualification"><xsl:with-param name="inputString" select="substring-after(@name, 'M:')" /></xsl:call-template>
					<xsl:value-of select="'()'"/>
				</a></h3>
				<div class="dspcont">
					<xsl:call-template name="contentDisplay" />
				</div>
			</xsl:when>
			<xsl:otherwise>
				<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)"><span class="dspchar">+</span> 
					<xsl:call-template name="removeQualification"><xsl:with-param name="inputString" select="$_fullName" /></xsl:call-template>
					<xsl:value-of select="'('"/>
					<xsl:call-template name="replaceCharsInString">
						<xsl:with-param name="stringIn" select="substring-after(@name, '(')"/>
						<xsl:with-param name="charsIn" select="','"/>
						<xsl:with-param name="charsOut" select="', '"/>
					</xsl:call-template>
				</a></h3>
				<div class="dspcont">
					<xsl:call-template name="contentDisplay" />
				</div>
			</xsl:otherwise>
		</xsl:choose>
	
</xsl:template>

<!-- Operator display -->
<xsl:template match="member[contains(@name, 'M:') and (contains(@name, 'op_') or contains(@name, 'op_'))]">
	<xsl:variable name="_parentName" select="substring-before(substring-after(@name, 'M:'), '.op')"/>
	
	<xsl:if test="contains(@name, 'Explicit')">	
		<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)"><span class="dspchar">+</span> 
		 explicit operator <xsl:value-of select="substring-after(@name, '~')"/> (<xsl:value-of select="substring-after(substring-before(@name, ')'), '(')"/>)
		</a></h3>
	</xsl:if>
	
	<xsl:if test="contains(@name, 'Implicit')">	
		<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)"><span class="dspchar">+</span> 
		 implicit operator <xsl:value-of select="substring-after(@name, '~')"/> (<xsl:value-of select="substring-after(substring-before(@name, ')'), '(')"/>)
		</a></h3>
	</xsl:if>
	
	<!-- Test for all other possible operators, and replace the mangled names with the appropriate signs -->
	<xsl:if test="not(contains(@name, 'Explicit')) and not(contains(@name, 'Implicit'))">
			<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)"><span class="dspchar">+</span> 
				<xsl:if test="contains(@name, 'op_Decrement')"><xsl:variable name="_opsign" select="'&#45;&#45;'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_Increment')"><xsl:variable name="_opsign" select="'++'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_UnaryNegation')"><xsl:variable name="_opsign" select="'-'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>         
			  <xsl:if test="contains(@name, 'op_UnaryPlus')"><xsl:variable name="_opsign" select="'+'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>             
			  <xsl:if test="contains(@name, 'op_LogicalNot')"><xsl:variable name="_opsign" select="'!'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_True')"><xsl:variable name="_opsign" select="'true'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>                     
			  <xsl:if test="contains(@name, 'op_False')"><xsl:variable name="_opsign" select="'false'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>              
			  <xsl:if test="contains(@name, 'op_OnesComplement')"><xsl:variable name="_opsign" select="'~'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>         
			  <xsl:if test="contains(@name, 'op_Addition')"><xsl:variable name="_opsign" select="'+'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>               
			  <xsl:if test="contains(@name, 'op_Subtraction')"><xsl:variable name="_opsign" select="'-'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_Division')"><xsl:variable name="_opsign" select="'/'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>               
			  <xsl:if test="contains(@name, 'op_Multiply')"><xsl:variable name="_opsign" select="'*'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>               
			  <xsl:if test="contains(@name, 'op_Modulus')"><xsl:variable name="_opsign" select="'%'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>                
			  <xsl:if test="contains(@name, 'op_BitwiseAnd')"><xsl:variable name="_opsign" select="'&amp;'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>             
			  <xsl:if test="contains(@name, 'op_ExclusiveOr')"><xsl:variable name="_opsign" select="'^'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_LeftShift')"><xsl:variable name="_opsign" select="'&lt;&lt;'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>             
			  <xsl:if test="contains(@name, 'op_RightShift')"><xsl:variable name="_opsign" select="'&gt;&gt;'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_BitwiseOr')"><xsl:variable name="_opsign" select="'|'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>              
			  <xsl:if test="contains(@name, 'op_Equality')"><xsl:variable name="_opsign" select="'=='" /> operator <xsl:value-of select="$_opsign"/></xsl:if>              
			  <xsl:if test="contains(@name, 'op_Inequality')"><xsl:variable name="_opsign" select="'!='" /> operator <xsl:value-of select="$_opsign"/></xsl:if>            
			  <xsl:if test="contains(@name, 'op_LessThanOrEqual')"><xsl:variable name="_opsign" select="'&lt;='" /> operator <xsl:value-of select="$_opsign"/></xsl:if>       
			  <xsl:if test="contains(@name, 'op_LessThan(')"><xsl:variable name="_opsign" select="'&lt;'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>               
			  <xsl:if test="contains(@name, 'op_GreaterThanOrEqual')"><xsl:variable name="_opsign" select="'&gt;='" /> operator <xsl:value-of select="$_opsign"/></xsl:if>    
			  <xsl:if test="contains(@name, 'op_GreaterThan(')"><xsl:variable name="_opsign" select="'&gt;'" /> operator <xsl:value-of select="$_opsign"/></xsl:if>
			  <xsl:call-template name="replaceCharsInString">
						<xsl:with-param name="stringIn" select="concat('(', substring-after(substring-before(@name, ')'), '('), ')')"/>
						<xsl:with-param name="charsIn" select="','"/>
						<xsl:with-param name="charsOut" select="', '"/>
					</xsl:call-template>
			  
		</a></h3>
	</xsl:if>
		
	<div class="dspcont">
		<xsl:call-template name="contentDisplay" />
	</div>
</xsl:template>

<!-- Enum member display -->
<xsl:template match="member[contains(@name, 'F:')]">
	<xsl:variable name="_temp" select="substring-after(@name, '.')"/>
	
	<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)">
		<span class="dspchar">+</span> <xsl:call-template name="removeQualification"><xsl:with-param name="inputString" select="$_temp" /></xsl:call-template> </a></h3>
	<div class="dspcont">
		<xsl:call-template name="contentDisplay" />
	</div>
	
</xsl:template>

<!-- Property display -->
<xsl:template match="member[contains(@name, 'P:')]">
	<xsl:variable name="_temp" select="substring-after(@name, '.')"/>
	
	<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)">
		<span class="dspchar">+</span> <xsl:call-template name="removeQualification"><xsl:with-param name="inputString" select="$_temp" /></xsl:call-template> </a></h3>
	<div class="dspcont">
		<xsl:call-template name="contentDisplay" />
	</div>
</xsl:template>

<!-- Constructor display -->
<xsl:template name="constructorDisplay" match="member[contains(@name, '.#ctor') or contains(@name, '.#cctor')]">
	<!--<xsl:param name="constructorName"/>-->
	<!--<xsl:param name="typeName"/>-->
	<xsl:variable name="_temp" select="substring-after(@name, 'M:')"/>
	
	<!-- Check this constructor belongs to the current node -->
	<!-- <xsl:if test="starts-with($_temp, $typeName)"> -->
	<xsl:choose>
		<xsl:when test="contains($_temp, '.#ctor')">
			<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)">
 			<span class="dspchar">+</span>
			 	<xsl:variable name="_ctorName" select="concat(substring-before($_temp, '.#ctor'), substring-after($_temp, '.#ctor'))"/>
			 	<xsl:variable name="_unqualifiedName" select="substring-after($_ctorName, '.')"/>
			 	<xsl:variable name="_newName">
					<xsl:call-template name="replaceCharsInString">
						<xsl:with-param name="stringIn" select="$_unqualifiedName"/>
						<xsl:with-param name="charsIn" select="','"/>
						<xsl:with-param name="charsOut" select="', '"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:value-of select="$_newName"/>
			<br/></a></h3>
			<div class="dspcont">
				<xsl:call-template name="contentDisplay" />
				<br/>
			</div>
		</xsl:when>
		<xsl:when test="contains($_temp, '.#cctor')">
			<h3><a href="javascript:void(0)" class="dsphead1" onclick="dsp(this)">
 			<span class="dspchar">+</span>
			 	<xsl:variable name="_ctorName" select="concat(substring-before($_temp, '.#cctor'), substring-after($_temp, '.#cctor'))"/>
			 	<xsl:variable name="_unqualifiedName" select="substring-after($_ctorName, '.')"/>
			 	<xsl:variable name="_newName">
					<xsl:call-template name="replaceCharsInString">
						<xsl:with-param name="stringIn" select="$_unqualifiedName"/>
						<xsl:with-param name="charsIn" select="','"/>
						<xsl:with-param name="charsOut" select="', '"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:value-of select="$_newName"/>
			<br/></a></h3>
			<div class="dspcont">
				<xsl:call-template name="contentDisplay" />
				<br/>
			</div>
		
		</xsl:when>
	</xsl:choose>
	<!-- </xsl:if> -->
	
</xsl:template>

<!-- Content display -->
<!-- Displays all the tags under a member node -->
<xsl:template name="contentDisplay">
	
	<!-- Display summary -->
	<xsl:apply-templates select="summary" />
	
	<!-- Display parameters -->
	<xsl:if test="count(param) != 0">	
 		<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Parameters</a></h3>
 			<div class="dspcont">
 				<table class="paramtable">
 					<tr class="paramheader">
 						<td class="paramcell"><strong>Name</strong></td>
 						<td class="paramcell"><strong>Description</strong></td>
 					</tr>
 					<xsl:apply-templates select="param" />
 				</table>
 			</div>
 	</xsl:if>
 	
 	<!-- Display returns -->
 	<xsl:if test="count(returns) != 0">
 		<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Returns</a></h3>
 		<div class="dspcont"><xsl:apply-templates select="returns" /></div>
 	</xsl:if>
 	
 	<!-- Display remarks -->
	<xsl:apply-templates select="remarks" />
	
	<!-- Display example -->
	<xsl:apply-templates select="example" />
</xsl:template>

<!-- Parameter display -->
<xsl:template match="param">
	<tr>
		<td class="paramcell"><xsl:value-of select="@name" /></td>
		<td class="paramcell"><xsl:value-of select="." /></td>
	</tr>	
</xsl:template>

<!-- Returns display -->
<xsl:template match="returns">
	<xsl:value-of select="." />
</xsl:template>

<!-- Summary display -->
<xsl:template name="summaryDisplay" match="summary">
	<xsl:variable name="summaryText" select="text()" />
	<xsl:variable name="stripSummary" select="normalize-space($summaryText)"/> <!--substring-after(., '&#x0D;')" />-->
	
	<!-- If the text is non-empty, put a heading for the summary here -->
	<xsl:choose>
		<xsl:when test="string-length(normalize-space($stripSummary)) != 0">
			<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Summary</a></h3>		
 			<div class="dspcont">
				<xsl:call-template name="splitReturns">
					<xsl:with-param name="inputString" select="$summaryText"/>
				</xsl:call-template>
				<xsl:if test="string-length(./code) != 0">
					<br /><br /><xsl:apply-templates select="./code" />	
				</xsl:if>
			</div>
		</xsl:when>
		<xsl:when test="string-length(normalize-space($stripSummary)) = 0 and string-length(./code) != 0">		
				<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Summary</a></h3>
				<div class="dspcont"><xsl:apply-templates select="./code" /></div>
		</xsl:when>
	</xsl:choose>
	
</xsl:template>

<!-- Remarks display -->
<xsl:template match="remarks">
	<xsl:variable name="summaryText" select="text()" />
	<xsl:variable name="stripSummary" select="normalize-space($summaryText)"/>
	
	<!-- If the text is non-empty, put a heading for the summary here -->
	<xsl:if test="string-length(normalize-space($stripSummary)) != 0">
		<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Remarks</a></h3>	
	</xsl:if>
	
	<div class="dspcont">
		<xsl:call-template name="splitReturns">
			<xsl:with-param name="inputString" select="."/>
		</xsl:call-template>
		<xsl:if test="string-length(./code) != 0">
			<br /><br /><xsl:apply-templates select="./code" />	
		</xsl:if>
	</div>
</xsl:template>

<!-- Example display -->
<xsl:template match="example">
	<xsl:variable name="stripSummary" select="normalize-space(text())"/> <!--substring-after(., '&#x0D;')" />-->
	
	<!-- If the text is non-empty, put a heading for the example here -->
	<xsl:choose>
		<xsl:when test="string-length(normalize-space($stripSummary)) != 0">
			<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Example</a></h3>		
 			<div class="dspcont">
				<xsl:call-template name="splitReturns">
					<xsl:with-param name="inputString" select="$stripSummary"/>
				</xsl:call-template>
				<xsl:if test="string-length(./code) != 0">
					<br /><br /><xsl:apply-templates select="./code" />	
				</xsl:if>
			</div>
		</xsl:when>
		<xsl:when test="string-length(normalize-space($stripSummary)) = 0 and string-length(./code) != 0">		
				<h3><a href="javascript:void(0)" class="dsphead" onclick="dsp(this)">
 			<span class="dspchar">+</span> Example</a></h3>
				<div class="dspcont"><xsl:apply-templates select="./code" /></div>
		</xsl:when>
	</xsl:choose>
	
</xsl:template>


<!-- Code display -->
<xsl:template match="code">
	<xsl:variable name="_codeText" select="text()"/>
	
	<!-- If the text is non-empty, put a heading for the summary here -->
	<xsl:if test="string-length(normalize-space($_codeText)) != 0">
		<div class="dspcode">
			<xsl:call-template name="splitReturns">
				<xsl:with-param name="inputString" select="$_codeText"/>
			</xsl:call-template>
		</div>
	</xsl:if>
		
</xsl:template>


<!-- Evaluates to the unqualified name of the element passed in (for properties and types) -->
<xsl:template name="removeQualification">
	<xsl:param name="inputString"/>
	
	<xsl:choose>
		<xsl:when test="not(contains($inputString, '.'))">
			<xsl:value-of select="$inputString" />
		</xsl:when>
		<xsl:otherwise>
			<xsl:call-template name="removeQualification">
				<xsl:with-param name="inputString" select="substring-after($inputString, '.')" />	
			</xsl:call-template>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<!-- Adds <br> tags where line-feed characters appear in the input, for multi-line elements -->
<!-- Normally, this would be trivial if Firefox supported disable-output-escaping on value-of statements -->
<!-- Non disable-output-escaping solution -->
<xsl:template name="splitReturns">
	<xsl:param name="inputString"/>
	
	<xsl:choose>
		
		<!-- Test to see if the input string still contains unprocessed stuff -->
		<xsl:when test="string-length($inputString)!=0">
			
			<xsl:variable name="_substringBefore" select="normalize-space(substring-before($inputString, '&#x0A;'))" />
			
			<!-- If the previous substring contains a carriage return before the text, ignore it -->
			<xsl:if test="string-length($_substringBefore) != 0">
				<xsl:choose>
					<xsl:when test="starts-with($_substringBefore, '//')">
						<span class="dspcomment"><xsl:value-of select="$_substringBefore" /></span><br></br>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$_substringBefore" /><br></br>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			
			<!-- Make an exception to that rule where double carriage returns exist interior to the text -->
			<xsl:variable name="_substringAfter" select="substring-after($inputString, '&#x0A;')" />
			<xsl:variable name="_nextSubBefore" select="substring-before($_substringAfter, '&#x0A;')" />
			<xsl:variable name="_nextSubAfter" select="substring-after($_substringAfter, '&#x0A;')" />
			
			<xsl:if test="string-length(normalize-space($_nextSubBefore)) = 0 and string-length(normalize-space($_nextSubAfter)) != 0">
				<br></br>
			</xsl:if>
			
			<!-- Add carriage returns to the remainder of the text -->
			<xsl:choose>
				<xsl:when test="string-length($_substringAfter) != 0">
					<xsl:call-template name="splitReturns">
						<xsl:with-param name="inputString" select="$_substringAfter" />	
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:if test="string-length(normalize-space($inputString)) != 0">
						<xsl:value-of select="$inputString"/>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:when>	
	</xsl:choose>
</xsl:template>

<!-- String replacement template -->
<xsl:template name="replaceCharsInString">
  <xsl:param name="stringIn"/>
  <xsl:param name="charsIn"/>
  <xsl:param name="charsOut"/>
  <xsl:choose>
    <xsl:when test="contains($stringIn,$charsIn)">
      <xsl:value-of select="concat(substring-before($stringIn,$charsIn),$charsOut)"/>
      <xsl:call-template name="replaceCharsInString">
        <xsl:with-param name="stringIn" select="substring-after($stringIn,$charsIn)"/>
        <xsl:with-param name="charsIn" select="$charsIn"/>
        <xsl:with-param name="charsOut" select="$charsOut"/>
      </xsl:call-template>
    </xsl:when>
    <xsl:otherwise>
      <xsl:value-of select="$stringIn"/>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

</xsl:stylesheet>
