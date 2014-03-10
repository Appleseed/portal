<%@ Page Language="VB" ContentType="image/jpeg" Debug="true" %>
<%@ import Namespace="ELB" %>
<%@ import Namespace="System.Drawing" %>
<%@ import Namespace="System.Drawing.Imaging" %>
<script runat="server">
	' This script is part of the EasyListBox server control.
	'  Purchase and licensing details can be found at http://EasyListBox.com .

    Public Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

            Dim myELB As New EasyListBoxArrowButton()
			' myELB.WinXP = True ' Force XP-style buttons (don't forget to set your border to solid!)
            Try
                Dim bmpELB As Bitmap = myELB.CreateImage()
                Dim MS as System.IO.MemoryStream = New System.IO.MemoryStream()
                Response.Clear()
                Response.ContentType = "Image/Png"
                bmpELB.Save(MS,System.Drawing.Imaging.ImageFormat.Png)
                Dim buffer as Byte() = MS.ToArray()
                Response.OutputStream.Write(buffer,0,buffer.Length)
                Response.End()

            Catch ex As System.Exception
				Response.ContentType = "text/html"
                litErrorMsg.Visible = True
                litErrorMsg.Text = "Error generating listbox arrow: <br />" & ex.ToString
            End Try

    End Sub

</script>
<asp:Literal ID="litErrorMsg" Visible="False" Runat="Server" />
