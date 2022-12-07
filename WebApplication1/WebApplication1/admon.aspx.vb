Imports System.Data
Imports System.Data.SqlClient
Imports Ionic.Zip
Imports System.IO
Imports System
Imports System.Threading
Imports Microsoft.Office.Interop.Excel
Imports iText.Forms
Imports iText.IO.Font
Imports iText.Kernel.Font
Imports iText.Kernel.Pdf
Imports iText.Forms.Fields

Public Class WebForm32
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader
    Dim pkPorcen
    Dim idePkLimite
    Dim idePkPorcen
    Dim PKplanFecha
    Dim PKplanElplan
    Dim pkDesctoPorcen
    Dim pkDesctoCod
    Dim savePath

    Private Sub cuentaRegistros()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM iva"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        ivaNregs.Text = FormatNumber(v.ToString, 0) + " Registros"
        GridView1.SelectedIndex = -1
    End Sub
    Private Sub cuentaRegistrosIde()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM ideConf"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        ideNregs.Text = FormatNumber(v.ToString, 0) + " Registros"
        GridView2.SelectedIndex = -1
    End Sub

    Private Sub cuentaRegistrosDescto()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM desctos"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        desctoNregs.Text = FormatNumber(v.ToString, 0) + " Registros"
        GridView4.SelectedIndex = -1
    End Sub
    Private Sub cuentaRegistrosPros()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM prospectos"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        prosNregs.Text = FormatNumber(v.ToString, 0) + " Registros"
        GridView5.SelectedIndex = -1
    End Sub

    Protected Sub ingresar_Click(sender As Object, e As EventArgs) Handles ingresar.Click
        myCommand = New SqlCommand("SELECT id FROM admin WHERE nombre='" + pass.Text.Trim + "'")
        Dim v = ExecuteScalarFunction(myCommand)
        If IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Contraseña incorrecta');</script>")
            btnOculto_Click(sender, e)
        Else
            Session("admonIn") = "1"
            'UpdatePanel3.Update()
            panel1_ModalPopupExtender.Hide()
        End If

    End Sub
    Protected Sub btnOculto_Click(sender As Object, e As EventArgs) Handles btnOculto.Click
        '                UpdatePanel3.Update()
        panel1_ModalPopupExtender.Show()
        Panel5.Style.Remove("display")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager1.RegisterPostBackControl(ingresar)
        ScriptManager1.RegisterPostBackControl(btnOculto)

        If Not IsPostBack Then

            If Not String.IsNullOrEmpty(Request.QueryString("lan")) Then
                If Request.QueryString("lan") = "1" Then 'red local
                    If Session("admonIn") = "1" Then

                    Else
                        Session("admonIn") = "1"
                        btnOculto_Click(sender, e)
                    End If
                Else
                    Response.Write("<script language='javascript'>alert('Acceso denegado por su ubicación/forma de acceso');</script>")
                    Response.Write("<script>location.href='Login.aspx';</script>")
                End If
            Else
                If Session("admonIn") <> "1" Then
                    Response.Write("<script language='javascript'>alert('Acceso denegado por su ubicación/forma de acceso');</script>")
                    Response.Write("<script>location.href='Login.aspx';</script>")
                End If

            End If
        End If

        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)

        'If Session("runAsAdmin") = "1" Then
        '    Dim item As MenuItem = MenuTabs1.FindItem("IVA")
        '    item.Parent.ChildItems.Remove(item)
        '    item = MenuTabs1.FindItem("IDE")
        '    item.Parent.ChildItems.Remove(item)
        '    item = MenuTabs1.FindItem("PLANES")
        '    item.Parent.ChildItems.Remove(item)
        '    item = MenuTabs1.FindItem("DESCUENTOS")
        '    item.Parent.ChildItems.Remove(item)
        'End If

        MultiView1.ActiveViewIndex = Request.QueryString("v")

        If MultiView1.ActiveViewIndex = 0 Then 'iva
            Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll.ClientID + "','scrollPos');", True)
        ElseIf MultiView1.ActiveViewIndex = 1 Then 'ide
            Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll2.ClientID + "','scrollPos2');", True)
        ElseIf MultiView1.ActiveViewIndex = 2 Then 'planes
            Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll3.ClientID + "','scrollPos3');", True)
        ElseIf MultiView1.ActiveViewIndex = 3 Then  'desctos
            Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll4.ClientID + "','scrollPos4');", True)
        Else
            Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll5.ClientID + "','scrollPos5');", True)
        End If

        Dim q As String
        q = "SELECT ivaPorcen FROM actuales"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            actualIva.Text = v.ToString()
        End If
        cuentaRegistros()

        q = "SELECT ideLim, idePorcen FROM actuales"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            If dr.Read() Then
                q = "SELECT id FROM ideConf where limite='" + dr("ideLim").ToString + "' and porcen='" + dr("idePorcen").ToString + "'"
                myCommand = New SqlCommand(q)
                v = ExecuteScalarFunction(myCommand)
                actualIde.Text = v.ToString()
            End If
        End Using
        cuentaRegistrosIde()

        GridView3.SelectedIndex = -1


        cuentaRegistrosDescto()
        GridView4.SelectedIndex = -1

        cuentaRegistrosPros()
        GridView5.SelectedIndex = -1
    End Sub

    Private Sub WebForm1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'myConnection.Close()
    End Sub

    Private Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim row As GridViewRow = GridView1.SelectedRow
        id.Text = row.Cells(1).Text
        porcen.Text = row.Cells(2).Text
    End Sub

    Private Sub GridView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.SelectedIndexChanged
        Dim row As GridViewRow = GridView2.SelectedRow
        idIde.Text = row.Cells(1).Text
        limite.Text = row.Cells(2).Text
        idePorcen.Text = row.Cells(3).Text
    End Sub

    Private Sub GridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView3.SelectedIndexChanged
        Dim row As GridViewRow = GridView3.SelectedRow
        idPlan.Text = row.Cells(1).Text
        fecha.Text = row.Cells(2).Text
        elPlan.Text = Server.HtmlDecode(row.Cells(3).Text)
        precioBaseMes.Text = row.Cells(4).Text
        ivaPlan.Text = row.Cells(5).Text
        inscrip.Text = row.Cells(6).Text
    End Sub
    Private Sub GridView4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.SelectedIndexChanged
        Dim row As GridViewRow = GridView4.SelectedRow
        idDescto.Text = row.Cells(1).Text
        cod.Text = Server.HtmlDecode(row.Cells(2).Text)
        If row.Cells(3).Text = "False" Then
            Caduca.Checked = False
        Else
            Caduca.Checked = True
        End If
        If row.Cells(4).Text = "" Or row.Cells(4).Text = "&nbsp;" Then
            fechaCaducidad.Text = ""
        Else
            fechaCaducidad.Text = row.Cells(4).Text
        End If
        desctoPorcen.Text = row.Cells(5).Text
        tipo.Text = Server.HtmlDecode(row.Cells(6).Text)
        plan.Text = Server.HtmlDecode(row.Cells(7).Text)
        If row.Cells(8).Text = "False" Then
            inscripGratis.Checked = False
        Else
            inscripGratis.Checked = True
        End If
        If row.Cells(9).Text = "False" Then
            regularizacion.Checked = False
        Else
            regularizacion.Checked = True
        End If
        If row.Cells(10).Text = "False" Then
            anticipadas.Checked = False
        Else
            anticipadas.Checked = True
        End If
        nDeclContratadas.Text = row.Cells(11).Text
        duracionMeses.Text = row.Cells(12).Text
        If row.Cells(13).Text = "" Or row.Cells(13).Text = "&nbsp;" Then
            idPreRequisito.Text = ""
        Else
            idPreRequisito.Text = row.Cells(13).Text
        End If
        If row.Cells(14).Text = "" Or row.Cells(14).Text = "&nbsp;" Then
            inscripMonto.Text = ""
        Else
            inscripMonto.Text = row.Cells(14).Text
        End If
    End Sub
    Protected Sub add_Click(ByVal sender As Object, ByVal e As EventArgs) Handles add.Click
        If validaVacios() < 1 Then
            Exit Sub
        End If

        If validaDupl() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "INSERT INTO iva(porcen) VALUES(" + Trim(porcen.Text) + ")"
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        'refrescar grid
        id.Text = "ID"
        porcen.Text = ""
        GridView1.DataBind()
        cuentaRegistros()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub

    Protected Sub addIde_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addIde.Click
        If validaVaciosIde() < 1 Then
            Exit Sub
        End If

        If validaDuplIde() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "INSERT INTO ideConf(limite,porcen) VALUES('" + Trim(limite.Text) + "','" + Trim(idePorcen.Text) + "')"
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)


        'refrescar grid
        idIde.Text = "ID"
        limite.Text = ""
        idePorcen.Text = ""
        GridView2.DataBind()
        cuentaRegistrosIde()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub
    Protected Sub addPlan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addPlan.Click
        If validaVaciosPlan() < 1 Then
            Exit Sub
        End If

        If validaDuplPlan() < 1 Then
            Exit Sub
        End If

        Dim q As String
        q = "select * from iva where porcen in (select ivaPorcen from actuales)"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            q = "INSERT INTO planes(fecha,elplan,precioBaseMes,idIva,iva,inscrip) VALUES('" + Format(Now(), "yyyy-MM-dd") + "','" + Trim(elPlan.Text.ToUpper) + "','" + precioBaseMes.Text.Trim + "'," + dr("id").ToString + ",'" + dr("porcen").ToString + "','" + inscrip.Text.Trim + "')"
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
        End Using


        'refrescar grid
        idPlan.Text = "ID"
        fecha.Text = ""
        elPlan.Text = ""
        precioBaseMes.Text = ""
        ivaPlan.Text = ""
        inscrip.Text = ""
        GridView3.DataBind()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub
    Protected Sub addDescto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addDescto.Click
        If validaVaciosDescto() < 1 Then
            Exit Sub
        End If

        If validaDuplDescto() < 1 Then
            Exit Sub
        End If
        Dim q As String
        Dim caducaV, inscripGratisV, regularizacionV, anticipadasV, idPreRequisitoV, inscripMontoV
        If inscripGratis.Checked = True Then
            inscripGratisV = "1"
        Else
            inscripGratisV = "0"
        End If
        If regularizacion.Checked = True Then
            regularizacionV = "1"
        Else
            regularizacionV = "0"
        End If
        If anticipadas.Checked = True Then
            anticipadasV = "1"
        Else
            anticipadasV = "0"
        End If
        If nDeclContratadas.Text.Trim = "" Then
            nDeclContratadas.Text = "0"
        End If
        If duracionMeses.Text.Trim = "" Then
            duracionMeses.Text = "0"
        End If
        If idPreRequisito.Text.Trim <> "" Then
            idPreRequisitoV = idPreRequisito.Text.Trim
        Else
            idPreRequisitoV = "NULL"
        End If
        If inscripMonto.Text.Trim <> "" Then
            inscripMontoV = inscripMonto.Text.Trim
        Else
            inscripMontoV = "NULL"
        End If

        If Caduca.Checked = True Then
            caducaV = "1"
            q = "INSERT INTO desctos(cod,caduca,fechaCaducidad,porcen,tipo,elPlan,inscripGratis,regularizacion,anticipadas,nDeclContratadas,duracionMeses,idPreRequisito,inscripMonto) VALUES('" + cod.Text.Trim.ToUpper + "'," + caducaV + ",'" + Format(Convert.ToDateTime(fechaCaducidad.Text.Trim), "yyyy-MM-dd") + "','" + Trim(desctoPorcen.Text) + "','" + tipo.Text.Trim + "','" + plan.Text + "'," + inscripGratisV + "," + regularizacionV + "," + anticipadasV + "," + nDeclContratadas.Text.Trim + "," + duracionMeses.Text.Trim + ", " + idPreRequisitoV + "," + Replace(Trim(inscripMontoV), ",", "") + ")"
        Else
            caducaV = "0"
            q = "INSERT INTO desctos(cod,caduca,porcen,tipo,elPlan,inscripGratis,regularizacion,anticipadas,nDeclContratadas,duracionMeses,idPreRequisito,inscripMonto) VALUES('" + cod.Text.Trim.ToUpper + "'," + caducaV + ",'" + Trim(desctoPorcen.Text) + "','" + tipo.Text.Trim + "','" + plan.Text + "'," + inscripGratisV + "," + regularizacionV + "," + anticipadasV + "," + nDeclContratadas.Text.Trim + "," + duracionMeses.Text.Trim + "," + idPreRequisitoV + "," + Replace(Trim(inscripMontoV), ",", "") + ")"
        End If

        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        'refrescar grid
        idDescto.Text = "ID"
        cod.Text = ""
        Caduca.Checked = False
        fechaCaducidad.Text = ""
        desctoPorcen.Text = ""
        tipo.Text = "VACIO"
        elPlan.Text = "VACIO"
        inscripGratis.Checked = False
        inscripMonto.Text = ""
        regularizacion.Checked = False
        anticipadas.Checked = False
        nDeclContratadas.Text = ""
        duracionMeses.Text = ""
        idPreRequisito.Text = ""
        GridView4.DataBind()
        cuentaRegistrosDescto()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub
    Private Function validaVacios() As Integer
        If Trim(porcen.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el porcentaje');</script>")
            porcen.Focus()
            Return 0
        End If

        Return 1
    End Function
    Private Function validaVaciosIde() As Integer
        If Trim(limite.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el limite de Ide');</script>")
            limite.Focus()
            Return 0
        End If
        If Trim(idePorcen.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el porcentaje de Ide');</script>")
            idePorcen.Focus()
            Return 0
        End If

        Return 1
    End Function
    Private Function validaVaciosPlan() As Integer
        If Trim(elPlan.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el plan');</script>")
            elPlan.Focus()
            Return 0
        End If
        If Trim(precioBaseMes.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el precio base mensual');</script>")
            precioBaseMes.Focus()
            Return 0
        End If
        If Trim(inscrip.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la inscripcion');</script>")
            inscrip.Focus()
            Return 0
        End If
        Return 1
    End Function
    Private Function validaVaciosDescto() As Integer
        If Trim(cod.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el codigo');</script>")
            cod.Focus()
            Return 0
        End If
        If Caduca.Checked = True And Trim(fechaCaducidad.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la fecha de caducidad');</script>")
            fechaCaducidad.Focus()
            Return 0
        End If
        If fechaCaducidad.Text.Trim <> "" Then
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fechaCaducidad.Text.Trim) Then
                If Not DateTime.TryParse(fechaCaducidad.Text.Trim, dtnow) Then
                    Response.Write("<script language='javascript'>alert('fecha caducidad invalida');</script>")
                    fechaCaducidad.Focus()
                    Return 0
                End If
            Else
                Response.Write("<script language='javascript'>alert('formato fecha de caducidad no valido (dd/mm/aaaa)');</script>")
                fechaCaducidad.Focus()
                Return 0
            End If
        End If
        If Trim(desctoPorcen.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el porcentaje o bien 0');</script>")
            desctoPorcen.Focus()
            Return 0
        End If
        If idPreRequisito.Text.Trim <> "" Then
            Dim q
            q = "SELECT id FROM desctos WHERE id=" + Trim(idPreRequisito.Text.Trim)
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                Response.Write("<script language='javascript'>alert('No se encontro id pre requisito en descuentos');</script>")
                Return 0
            End If
        End If
        If tipo.Text = "PROMOCION" And inscripGratis.Checked = True And elPlan.Text <> "PREMIUM" Then
            Response.Write("<script language='javascript'>alert('Inscripcion gratis solo para planes premium');</script>")
            Return 0
        End If

        Return 1
    End Function
    Private Function validaDupl() As Integer
        Dim q
        q = "SELECT id FROM iva WHERE porcen='" + Trim(porcen.Text) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ese porcentaje de iva ya existe');</script>")
            Return 0
        End If

        Return 1
    End Function
    Private Function validaDuplPlan() As Integer
        Dim q
        q = "SELECT id FROM planes WHERE elplan='" + Trim(elPlan.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ese plan ya existe');</script>")
            Return 0
        End If

        Return 1
    End Function
    Private Function validaDuplIde() As Integer
        Dim q
        q = "SELECT limite FROM ideConf WHERE limite='" + Trim(limite.Text) + "' and porcen='" + Trim(idePorcen.Text) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Esa especificacion de Ide ya existe');</script>")
            Return 0
        End If

        Return 1
    End Function
    Private Function validaDuplDescto() As Integer
        Dim q
        If tipo.Text = "REG" Then
            q = "SELECT id FROM desctos WHERE cod='" + Trim(cod.Text.Trim.ToUpper) + "' AND porcen='" + desctoPorcen.Text.Trim + "'"
        Else
            q = "SELECT id FROM desctos WHERE cod='" + Trim(cod.Text) + "'"
        End If
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ese codigo de descuento ya existe');</script>")
            Return 0
        End If

        'q = "SELECT id FROM desctos WHERE tipo='PROMO' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad"
        'myCommand = New SqlCommand(q, myConnection)
        'dr = myCommand.ExecuteReader()
        'If dr.Read() And tipo.Text = "PROMO" Then
        '    If CDate(fechaCaducidad.Text.Trim) >= CDate(Now()) Then
        '        dr.Close()
        '        Response.Write("<script language='javascript'>alert('Ya hay una promoción vigente');</script>")
        '        Return 0
        '    End If
        'End If
        'dr.Close()


        Return 1
    End Function
    Private Function validaDuplMod() As Integer
        Dim q
        q = "SELECT porcen FROM iva WHERE ID='" + Trim(id.Text) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        pkPorcen = v.ToString()

        q = "SELECT id FROM iva WHERE porcen='" + Trim(porcen.Text) + "'"
        myCommand = New SqlCommand(q)
        v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) And (pkPorcen <> Trim(porcen.Text)) Then   'if dr.Read() and (pkPorcen <> trim(porcen.text) or pk2 <>trim(campo2)) then
            Response.Write("<script language='javascript'>alert('Porcentaje ya está en uso');</script>")
            Return 0
        End If

        Return 1
    End Function

    Private Function validaDuplModIde() As Integer
        Dim q
        q = "SELECT limite,porcen FROM ideConf WHERE ID='" + Trim(idIde.Text) + "'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            idePkLimite = dr("limite").ToString()
            idePkPorcen = dr("porcen").ToString()
        End Using

        q = "SELECT id FROM ideConf WHERE limite='" + Trim(limite.Text) + "' and porcen='" + Trim(idePorcen.Text) + "'" 'and->1 col puede repet en otro reg
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) And (idePkPorcen <> Trim(idePorcen.Text) Or idePkLimite <> Trim(limite.Text)) Then
            Response.Write("<script language='javascript'>alert('Esa especificacion de IDe ya está en uso');</script>")
            Return 0
        End If

        Return 1
    End Function

    Private Function validaDuplModPlan() As Integer
        Dim q
        q = "SELECT elplan FROM planes WHERE ID='" + Trim(idPlan.Text) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        PKplanElplan = v.ToString()

        q = "SELECT id FROM planes WHERE elplan='" + Trim(elPlan.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q)
        v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) And (PKplanElplan <> Trim(elPlan.Text.ToUpper)) Then
            Response.Write("<script language='javascript'>alert('Esa plan ya está en uso');</script>")
            Return 0
        End If

        Return 1
    End Function
    Private Function validaDuplModDescto() As Integer
        Dim q
        q = "SELECT cod,porcen FROM desctos WHERE ID='" + Trim(idDescto.Text) + "'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()

            pkDesctoPorcen = dr("porcen").ToString()
            pkDesctoCod = dr("cod").ToString()
        End Using

        If tipo.Text = "REG" Then
            q = "SELECT id FROM desctos WHERE cod='" + Trim(cod.Text.Trim.ToUpper) + "' AND porcen='" + desctoPorcen.Text.Trim + "'"
        Else
            q = "SELECT id FROM desctos WHERE cod='" + Trim(cod.Text) + "'"
        End If
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If tipo.Text = "REG" Then
            If Not IsNothing(v) And (pkDesctoPorcen <> Trim(desctoPorcen.Text) And pkDesctoCod <> Trim(cod.Text)) Then   'if dr.Read() and (pkPorcen <> trim(porcen.text) or pk2 <>trim(campo2)) then
                Response.Write("<script language='javascript'>alert('Codigo ya está en uso');</script>")
                Return 0
            End If
        Else
            If dr.Read() And (pkDesctoCod <> Trim(cod.Text)) Then   'if dr.Read() and (pkPorcen <> trim(porcen.text) or pk2 <>trim(campo2)) then
                Response.Write("<script language='javascript'>alert('Codigo ya está en uso');</script>")
                Return 0
            End If
        End If

        'q = "SELECT id FROM desctos WHERE id<>" + id.Text + " and tipo='PROMO' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad"
        'myCommand = New SqlCommand(q, myConnection)
        'dr = myCommand.ExecuteReader()
        'If dr.Read() And tipo.Text = "PROMO" Then
        '    If CDate(fechaCaducidad.Text.Trim) >= CDate(Now()) Then
        '        dr.Close()
        '        Response.Write("<script language='javascript'>alert('Ya hay una promoción vigente');</script>")
        '        Return 0
        '    End If
        'End If
        'dr.Close()

        Return 1
    End Function
    Protected Sub defActualIva_Click(ByVal sender As Object, ByVal e As EventArgs) Handles defActualIva.Click
        If id.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        Dim q
        q = "UPDATE actuales SET ivaPorcen=" + Trim(porcen.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        Response.Write("<script language='javascript'>alert('Actualizado correctamente');</script>")
        actualIva.Text = Trim(porcen.Text)
        pkPorcen = Trim(porcen.Text)
    End Sub

    Protected Sub defActualIde_Click(ByVal sender As Object, ByVal e As EventArgs) Handles defActualIde.Click
        If idIde.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        Dim q
        q = "UPDATE actuales SET ideLim='" + Trim(limite.Text) + "', idePorcen='" + Trim(idePorcen.Text) + "'"
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        Response.Write("<script language='javascript'>alert('Actualizado correctamente');</script>")
        actualIde.Text = Trim(idIde.Text)
        idePkLimite = Trim(limite.Text)
        idePkPorcen = Trim(idePorcen.Text)
    End Sub

    Protected Sub edit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles edit.Click
        If id.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        If validaVacios() < 1 Then
            Exit Sub
        End If

        If validaDuplMod() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "UPDATE iva SET porcen=" + Trim(porcen.Text) + " WHERE id=" + id.Text
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        If actualIva.Text = pkPorcen Then       'if actualIva.text = pkPorcen and act2=pk2 then
            q = "UPDATE actuales SET IvaPorcen=" + Trim(porcen.Text)
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
            actualIva.Text = Trim(porcen.Text)
            pkPorcen = Trim(porcen.Text)
        End If

        'refrescar grid
        id.Text = "ID"
        porcen.Text = ""
        GridView1.DataBind()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")
    End Sub

    Protected Sub editIde_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editIde.Click
        If idIde.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro del ide');</script>")
            Exit Sub
        End If
        If validaVaciosIde() < 1 Then
            Exit Sub
        End If

        If validaDuplModIde() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "UPDATE ideConf SET limite='" + Trim(limite.Text) + "',porcen='" + Trim(idePorcen.Text) + "' WHERE id=" + idIde.Text
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)


        If actualIde.Text = idIde.Text Then
            q = "UPDATE actuales SET ideLimite='" + Trim(limite.Text) + "',idePorcen='" + Trim(idePorcen.Text) + "'"
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
            idePkLimite = Trim(limite.Text)
            idePkPorcen = Trim(idePorcen.Text)
        End If

        'refrescar grid
        idIde.Text = "ID"
        limite.Text = ""
        idePorcen.Text = ""
        GridView2.DataBind()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")
    End Sub

    Protected Sub editPlan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editPlan.Click
        If idPlan.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro de plan');</script>")
            Exit Sub
        End If
        If validaVaciosPlan() < 1 Then
            Exit Sub
        End If

        If validaDuplModPlan() < 1 Then
            Exit Sub
        End If

        Dim q As String
        q = "select * from iva where porcen in (select ivaPorcen from actuales)"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            q = "UPDATE planes SET fecha='" + Format(Now(), "yyyy-MM-dd") + "',elplan='" + Trim(elPlan.Text.ToUpper) + "',precioBaseMes='" + precioBaseMes.Text.Trim.ToUpper + "',idIva=" + dr("id").ToString + ",iva='" + ivaPlan.Text.Trim.ToString + "',inscrip='" + inscrip.Text.ToString + "' WHERE id=" + idPlan.Text
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
        End Using

        'refrescar grid
        idPlan.Text = "ID"
        fecha.Text = ""
        elPlan.Text = ""
        precioBaseMes.Text = ""
        ivaPlan.Text = ""
        inscrip.Text = ""
        GridView3.DataBind()
        Response.Write("<script language='javascript'>alert('Actualizacion exitosa');</script>")
    End Sub
    Protected Sub editDescto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editDescto.Click
        If idDescto.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        If validaVaciosDescto() < 1 Then
            Exit Sub
        End If

        If validaDuplModDescto() < 1 Then
            Exit Sub
        End If
        Dim q As String, fechaCaducidadV, caducaV, inscripGratisV, regularizacionV, anticipadasV, idPreRequisitoV, inscripMontoV
        If Caduca.Checked = True Then
            caducaV = "1"
            fechaCaducidadV = ", fechaCaducidad='" + Format(Convert.ToDateTime(fechaCaducidad.Text.Trim), "yyyy-MM-dd") + "'"
        Else
            caducaV = "0"
            fechaCaducidadV = ""
        End If
        If inscripGratis.Checked = True Then
            inscripGratisV = "1"
        Else
            inscripGratisV = "0"
        End If
        If regularizacion.Checked = True Then
            regularizacionV = "1"
        Else
            regularizacionV = "0"
        End If
        If anticipadas.Checked = True Then
            anticipadasV = "1"
        Else
            anticipadasV = "0"
        End If
        If nDeclContratadas.Text.Trim = "" Then
            nDeclContratadas.Text = "0"
        End If
        If duracionMeses.Text.Trim = "" Then
            duracionMeses.Text = "0"
        End If
        If idPreRequisito.Text.Trim = "" Then
            idPreRequisitoV = "NULL"
        Else
            idPreRequisitoV = idPreRequisito.Text.Trim
        End If

        If inscripMonto.Text.Trim = "" Then
            inscripMontoV = "NULL"
        Else
            inscripMontoV = "'" + inscripMonto.Text.Trim + "'"
        End If

        q = "UPDATE desctos SET cod='" + cod.Text.Trim.ToUpper + "',caduca=" + caducaV + fechaCaducidadV + ",porcen='" + Trim(desctoPorcen.Text) + "', tipo='" + tipo.Text.Trim + "', elPlan='" + plan.Text.Trim + "', inscripGratis=" + inscripGratisV + ", regularizacion=" + regularizacionV + ", anticipadas=" + anticipadasV + ", nDeclContratadas=" + nDeclContratadas.Text.Trim + ", duracionMeses=" + duracionMeses.Text.Trim + ", idPreRequisito=" + idPreRequisitoV + ", inscripMonto=" + inscripMontoV + " WHERE id=" + idDescto.Text
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        'refrescar grid
        idDescto.Text = "ID"
        cod.Text = ""
        Caduca.Checked = False
        fechaCaducidad.Text = ""
        desctoPorcen.Text = ""
        tipo.Text = "VACIO"
        plan.Text = "VACIO"
        inscripGratis.Checked = False
        inscripMonto.Text = ""
        regularizacion.Checked = False
        anticipadas.Checked = False
        nDeclContratadas.Text = ""
        duracionMeses.Text = ""
        idPreRequisito.Text = ""
        GridView4.DataBind()
        Response.Write("<script language='javascript'>alert('Actualización exitosa');</script>")
    End Sub
    Protected Sub del_Click(ByVal sender As Object, ByVal e As EventArgs) Handles del.Click
        If id.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If


        'validar si esta siendo usado x FKs
        Dim q As String
        q = "SELECT idIva FROM planes WHERE idIva=" + Trim(id.Text)
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues lo esta usando un plan');</script>")
            Exit Sub
        End If

        'del cascadas
        If Trim(porcen.Text) = actualIva.Text Then 'borrando la actual
            Response.Write("<script language='javascript'>alert('Este es el registro en uso, para eliminarlo marque 1o otro como el actual');</script>")
            Exit Sub
        End If

        q = "DELETE FROM iva WHERE id=" + Trim(id.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        id.Text = "ID"
        porcen.Text = ""
        cuentaRegistros()
        GridView1.DataBind()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")


    End Sub

    Protected Sub delide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles delIde.Click
        If idIde.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro del ide');</script>")
            Exit Sub
        End If
        'validar si esta siendo usado x FKs
        Dim q As String
        q = "SELECT idIdeConf FROM ideMens WHERE idIdeConf=" + Trim(idIde.Text)
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues esta siendo usado en una declaracion mensual');</script>")
            Exit Sub
        End If
        q = "SELECT idIdeConf FROM ideAnual WHERE idIdeConf=" + Trim(idIde.Text)
        myCommand = New SqlCommand(q)
        v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues esta siendo usado en una declaracion anual');</script>")
            Exit Sub
        End If

        'del cascadas
        If Trim(idIde.Text) = actualIde.Text Then 'borrando la actual
            Response.Write("<script language='javascript'>alert('Este es el registro en uso, para eliminarlo marque 1o otro como el actual');</script>")
            Exit Sub
        End If

        q = "DELETE FROM ideConf WHERE id=" + Trim(idIde.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        idIde.Text = "ID"
        limite.Text = ""
        idePorcen.Text = ""
        cuentaRegistrosIde()
        GridView2.DataBind()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")


    End Sub
    Protected Sub delPlan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles delPlan.Click
        If idPlan.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro de planes');</script>")
            Exit Sub
        End If
        'validar si esta siendo usado x FKs
        Dim q As String
        q = "SELECT idPlan FROM contratos WHERE idPlan=" + Trim(idPlan.Text)
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues esta siendo usado en un contrato');</script>")
            Exit Sub
        End If

        'del cascadas

        q = "DELETE FROM planes WHERE id=" + Trim(idPlan.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        idPlan.Text = "ID"
        ivaPlan.Text = "ID"
        fecha.Text = ""
        elPlan.Text = ""
        precioBaseMes.Text = ""
        inscrip.Text = ""
        GridView3.DataBind()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")

    End Sub
    Protected Sub delDescto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles delDescto.Click
        If idDescto.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        'validar si esta siendo usado x FKs
        Dim q As String
        q = "SELECT * FROM desctosContra WHERE idDescto=" + Trim(idDescto.Text)
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            If dr.Read() Then
                Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues lo esta usando el contrato " + dr("idContra").ToString + "');</script>")
                Exit Sub
            End If
        End Using

        'del cascadas

        q = "DELETE FROM desctos WHERE id=" + Trim(idDescto.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        idDescto.Text = "ID"
        cod.Text = ""
        Caduca.Checked = False

        fechaCaducidad.Text = ""
        desctoPorcen.Text = ""
        tipo.Text = "VACIO"
        plan.Text = "VACIO"
        inscripGratis.Checked = False
        regularizacion.Checked = False
        anticipadas.Checked = False
        nDeclContratadas.Text = ""
        duracionMeses.Text = ""
        cuentaRegistrosDescto()
        GridView4.DataBind()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")

    End Sub
    Public Sub MenuTabs1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles MenuTabs1.MenuItemClick

    End Sub

    Protected Sub notificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles notificar.Click

        Dim q As String
        q = "SELECT nombre, correo FROM prospectos WHERE estatus IS NULL OR estatus='VA'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            Dim elcorreo2 As New System.Net.Mail.MailMessage
            Using elcorreo2
                elcorreo2.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                While dr.Read()
                    elcorreo2.Bcc.Add(dr("correo"))
                    myCommand = New SqlCommand("UPDATE prospectos SET estatus='NO' WHERE correo='" + dr("correo") + "'", myConnection)
                    myCommand.ExecuteNonQuery()
                End While
                elcorreo2.Subject = "Deseamos servirte y hacer equipo juntos para tus envios de declaraciones de IDE (ver propuesta)"
                elcorreo2.Body = "<html><body>" + prosTextoNotificar.Text + "<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
                elcorreo2.IsBodyHtml = True
                elcorreo2.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
                smpt.EnableSsl = True 'req p server gmail
                Try
                    smpt.Send(elcorreo2)
                    elcorreo2.Dispose()
                Catch ex As Exception
                    Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                    Exit Sub
                Finally
                    Response.Write("<script language='javascript'>alert('Mensaje enviado');</script>")
                End Try
            End Using
        End Using


        GridView5.DataBind()
    End Sub

    Protected Sub GridView5_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView5.SelectedIndexChanged
        Dim row As GridViewRow = GridView5.SelectedRow
        prosId.Text = row.Cells(1).Text
        prosEstatus.Text = row.Cells(5).Text
        factTx.Checked = row.Cells(6).Text
    End Sub

    Protected Sub prosModEstatus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles prosModEstatus.Click
        If prosId.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o el registro');</script>")
            Exit Sub
        End If

        Dim factTxVal
        If factTx.Checked = False Then
            factTxVal = "0"
        Else
            factTxVal = "1"
        End If

        myCommand = New SqlCommand("UPDATE prospectos SET estatus='" + prosEstatus.Text + "', factTx=" + factTxVal + " WHERE id=" + prosId.Text)
        ExecuteNonQueryFunction(myCommand)
        GridView5.DataBind()
        Response.Write("<script language='javascript'>alert('Modificación exitosa');</script>")

    End Sub

    Protected Sub enviarAsesoria_Click(ByVal sender As Object, ByVal e As EventArgs) Handles enviarAsesoria.Click

        If correo.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Introduzca el correo al cual enviar la asesoría');</script>")
            Exit Sub
        End If

        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(correo.Text.Trim.ToUpper)
        elcorreo.Subject = "Asesoría especializada del IDE"
        elcorreo.Body = "<html><body>Hola, Bienvenido<br><br> Descargue la asesoría escrita especializada que hemos organizado para Usted referente a cómo pagar el IDE, plazos para pagar y para declarar el IDE, cómo vincular a su declaración el IDE pagado e información relacionada, acceda a esta información bajando el archivo <a href='declaracioneside.com/asesoria.docx'>aquí</a><br><br>Atentamente <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            myCommand = New SqlCommand("UPDATE prospectos SET edoAsesoria='EN' WHERE correo='" + correo.Text.Trim.ToUpper + "'")
            ExecuteNonQueryFunction(myCommand)
            GridView5.DataBind()
            Response.Write("<script language='javascript'>alert('Asesoría enviada');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error enviando asesoría: " & ex.Message + ", intente mas tarde');</script>")
            Exit Sub
        End Try
    End Sub


    Protected Sub uuidGuardar_Click(sender As Object, e As EventArgs) Handles uuidGuardar.Click
        If uuidNumContrato.Text = "" Then
            Response.Write("<script language='javascript'>alert('Campos incompletos');</script>")
            Exit Sub
        End If

        Dim q
        q = "SELECT id FROM contratos WHERE id=" + uuidNumContrato.Text
        myCommand = New SqlCommand(q, myConnection)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
        Else
            uuidNumContrato.Focus()
            Response.Write("<script language='javascript'>alert('Contrato no localizado');</script>")
            Exit Sub
        End If

        If uuid.Text <> "" Then
            Dim expresion = "[a-f0-9A-F]{8}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{12}"
            If Not Regex.IsMatch(uuid.Text, expresion) Then
                Response.Write("<script language='javascript'>alert('formato uuid incorrecto');</script>")
                uuid.Focus()
                Exit Sub
            End If
        End If

        myCommand = New SqlCommand("UPDATE contratos SET uuid='" + uuid.Text + "' WHERE id=" + uuidNumContrato.Text)
        ExecuteNonQueryFunction(myCommand)
        Response.Write("<script language='javascript'>alert('Cambios guardados');</script>")
    End Sub

    Private Function subeArch(ByVal btn As String) As Integer
        If Not FileUpload1.HasFile Then
            Response.Write("<script language='javascript'>alert('No especificó el archivo a subir');</script>")
            Return 0
        End If

        Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)
        Dim extension As String = System.IO.Path.GetExtension(fileName)
        If InStr(fileName, "á") > 0 Or InStr(fileName, "é") > 0 Or InStr(fileName, "í") > 0 Or InStr(fileName, "ó") > 0 Or InStr(fileName, "ú") > 0 Or InStr(fileName, "Á") > 0 Or InStr(fileName, "É") > 0 Or InStr(fileName, "Í") > 0 Or InStr(fileName, "Ó") > 0 Or InStr(fileName, "Ú") > 0 Then
            Response.Write("<script language='javascript'>alert('Cambie el nombre del archivo para que no tenga acentos e intente de nuevo');</script>")
            Return 0
        End If
        If Not (extension = ".xls" Or extension = ".xlsx") Then
            Response.Write("<script language='javascript'>alert('El archivo debe ser formato Excel');</script>")
            Return 0
        End If

        If btn = "crearCot" Then
            crearCot.Enabled = False
        Else
            generico.Enabled = False
        End If

        progressbar1.Style("width") = "0px"
        statusImport.Text = ""

        savePath = "C:\SAT\"
        savePath += Now.ToString("dd-MM-yyyy_HH-mm-ss") + "-" + Server.HtmlEncode(FileUpload1.FileName)
        Try
            FileUpload1.SaveAs(savePath)
        Catch ex As Exception
            crearCot.Enabled = True
            Dim MSG = "<script language='javascript'>alert('" + ex.Message + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Return 0
        Finally
            FileUpload1.PostedFile.InputStream.Flush()
            FileUpload1.PostedFile.InputStream.Close()
            FileUpload1.FileContent.Dispose()
            FileUpload1.Dispose()
        End Try

        Session("error") = ""
        Session("barraN") = 1
        Session("barraIteracion") = 0

        progressbar1.Style("width") = "0px"
        lblAvance.Text = ""
        statusImport.Text = ""

        Return 1
    End Function

    Protected Sub crearCot_Click(sender As Object, e As EventArgs) Handles crearCot.Click
        If subeArch("crearCot") < 1 Then
            Exit Sub
        End If

        Dim objThread As New Thread(New System.Threading.ThreadStart(AddressOf DoTheWork))
        objThread.IsBackground = True
        objThread.Start()
        Session("Thread") = objThread

        Timer1.Enabled = True

        'DoTheWork()
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblAvance.Text = "Procesando " + Session("barraIteracion").ToString + " de " + Session("barraN").ToString + " incluido el encabezado"
        Dim ren = Session("barraIteracion")
        Dim rens = Session("barraN")
        Dim percent = Double.Parse(ren * 100 / rens).ToString("0")
        progressbar1.Style("width") = percent + "px"

        If rens = ren Or Session("error") <> "" Then
            Timer1.Dispose()
            Timer1.Enabled = False
            If Session("error") <> "" Then
                statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros incluido el encabezado"
                'File.Delete(h1.Value) 'el de excel
            Else
                statusImport.Text = " Cotizaciones creadas y enviadas por correo OK "
            End If

            crearCot.Enabled = True
            generico.Enabled = True
        End If
    End Sub

    Protected Sub DoTheWork()
        'px
        cargarCot()

        crearCot.Enabled = True

    End Sub

    Protected Sub DoTheWork2()
        'px
        cargarMailing()

        generico.Enabled = True

    End Sub

    Private Function cargarMailing() As Integer
        Dim objThread As Thread = CType(Session("Thread"), Thread)
        Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
        Dim w As Workbook

        Try
            w = excel.Workbooks.Open(savePath)
            Dim sheet As Worksheet = w.Sheets(1) 'i     'abrirá la 1er hoja del libro
            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row 'sin rens en bco

            Session("barraN") = nRensPre

            w.Close(False)   'cierro excel y trabajo con la var
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then
                Dim rens As Integer = nRensPre 'array.GetUpperBound(0)
                Dim fecha, correo, institucion, num, atn, tel, pu, cant, subt, iva, tot, ultEjer, pends, correoLogin, fechaCorta
                Dim q
                fecha = Now.ToString("dd-MM-yyyy_HH-mm-ss")
                fechaCorta = Left(fecha, 10)
                'num = "IDE JC " + DatePart(DateInterval.Year, Now).ToString + "_" + DatePart(DateInterval.Month, Now).ToString + "_" + DatePart(DateInterval.Day, Now).ToString + "_1"

                Dim dirDestino = "C:\SAT\AUTOCOT\" + fecha
                cant = "1"
                Dim oDir As New System.IO.DirectoryInfo(Server.MapPath("~"))
                oDir.Attributes = oDir.Attributes And Not IO.FileAttributes.ReadOnly
                Dim origen As String = Server.MapPath("~/COTfmto2021prospec.pdf")
                If (Not System.IO.Directory.Exists(dirDestino)) Then
                    System.IO.Directory.CreateDirectory(dirDestino)
                End If
                For ren As Integer = 2 To rens '1rens=encab 2o=datos
                    If Not array(ren, 1) Is Nothing Then
                        correo = array(ren, 1).ToString.ToUpper.Trim.Replace("'", "''")
                        correoLogin = correo
                    Else
                        Continue For
                    End If
                    If Not array(ren, 2) Is Nothing Then
                        institucion = array(ren, 2).ToString.ToUpper.Trim.Replace("'", "''")
                    Else
                        institucion = ""
                    End If
                    If Not array(ren, 3) Is Nothing Then
                        tel = array(ren, 3).ToString.ToUpper.Trim.Replace("'", "''")
                    Else
                        tel = ""
                    End If
                    If Not array(ren, 4) Is Nothing Then
                        atn = array(ren, 4).ToString.ToUpper.Trim.Replace("'", "''")
                    Else
                        atn = ""
                    End If

                    num = "IDE " + fecha + " " + (ren - 1).ToString

                    Dim destino As String = dirDestino + "\" + correoLogin + " " + num + ".pdf"
                    If File.Exists(destino) Then
                        File.Delete(destino)
                    End If

                    Dim pdfDoc As PdfDocument = New PdfDocument(New PdfReader(origen), New PdfWriter(destino))
                    Dim Form As PdfAcroForm = PdfAcroForm.GetAcroForm(pdfDoc, True)
                    Form.SetGenerateAppearance(True)
                    Dim font As PdfFont = PdfFontFactory.CreateFont(Server.MapPath("~/Calibri.ttf"), PdfEncodings.WINANSI)
                    Dim fontBold As PdfFont = PdfFontFactory.CreateFont(Server.MapPath("~/CalibriBold.ttf"), PdfEncodings.WINANSI)
                    Form.GetField("correo").SetValue(correo, font, 9.0F)
                    If institucion <> "" Then
                        Form.GetField("institucion").SetValue(institucion, fontBold, 9.0F)
                    End If
                    If tel <> "" Then
                        Form.GetField("tel").SetValue(tel, fontBold, 9.0F)
                    End If
                    If atn <> "" Then
                        Form.GetField("atn").SetValue(atn, fontBold, 9.0F)
                    Else
                        Form.GetField("atn").SetValue("A quien corresponda", fontBold, 9.0F)
                    End If

                    Form.GetField("fecha").SetValue(Left(fecha, 10), font, 9.0F)
                    Form.GetField("num").SetValue(num, font, 9.0F)
                    Form.FlattenFields()
                    pdfDoc.Close()

                    oDir.Attributes = oDir.Attributes And IO.FileAttributes.ReadOnly

                    'enviar correo
                    Dim elcorreo As New System.Net.Mail.MailMessage
                    elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                    'elcorreo.To.Add(correo)
                    elcorreo.Bcc.Add(correo)
                    elcorreo.Subject = "Cotización de Declaración de Depósitos en Efectivo"
                    elcorreo.Body = "
                    <html>
                        <head>
                        <meta charset='utf-8'>
                        <!-- utf-8 works for most cases -->
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <!-- Forcing initial-scale shouldn't be necessary -->
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <!-- Use the latest (edge) version of IE rendering engine -->
                        <title>EmailTemplate-Responsive</title>
                        <!-- The title tag shows in email notifications, like Android 4.4. -->
                        <!-- Please use an inliner tool to convert all CSS to inline as inpage or external CSS is removed by email clients -->
                        <!-- important in CSS is used to prevent the styles of currently inline CSS from overriding the ones mentioned in media queries when corresponding screen sizes are encountered -->

                        <!-- CSS Reset -->
                        <style type='text/css'>
                    /* What it does: Remove spaces around the email design added by some email clients. */
                          /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */
                    html,  body {
                    	margin: 0 !important;
                    	padding: 0 !important;
                    	height: 100% !important;
                    	width: 100% !important;
                    }
                    /* What it does: Stops email clients resizing small text. */
                    * {
                    	-ms-text-size-adjust: 100%;
                    	-webkit-text-size-adjust: 100%;
                    }
                    /* What it does: Forces Outlook.com to display emails full width. */
                    .ExternalClass {
                    	width: 100%;
                    }
                    /* What is does: Centers email on Android 4.4 */
                    div[style*='margin 16px 0'] {
                    	margin: 0 !important;
                    }
                    /* What it does: Stops Outlook from adding extra spacing to tables. */
                    table,  td {
                    	mso-table-lspace: 0pt !important;
                    	mso-table-rspace: 0pt !important;
                    }
                    /* What it does: Fixes webkit padding issue. Fix for Yahoo mail table alignment bug. Applies table-layout to the first 2 tables then removes for anything nested deeper. */
                    table {
                    	border-spacing: 0 !important;
                    	border-collapse: collapse !important;
                    	table-layout: fixed !important;
                    	margin: 0 auto !important;
                    }
                    table table table {
                    	table-layout: auto;
                    }
                    /* What it does: Uses a better rendering method when resizing images in IE. */
                    img {
                    	-ms-interpolation-mode: bicubic;
                    }
                    /* What it does: Overrides styles added when Yahoo's auto-senses a link. */
                    .yshortcuts a {
                    	border-bottom: none !important;
                    }
                    /* What it does: Another work-around for iOS meddling in triggered links. */
                    a[x-apple-data-detectors] {
                    	color: inherit !important;
                    }
                    </style>

                        <!-- Progressive Enhancements -->
                        <style type='text/css'>

                            /* What it does: Hover styles for buttons */
                            .button-td,
                            .button-a {
                                transition: all 100ms ease-in;
                            }
                            .button-td:hover,
                            .button-a:hover {
                                background: #555555 !important;
                                border-color: #555555 !important;
                            }

                            /* Media Queries */
                            @media screen and (max-width: 600px) {

                                .email-container {
                                    width: 100% !important;
                                }

                                /* What it does: Forces elements to resize to the full width of their container. Useful for resizing images beyond their max-width. */
                                .fluid,
                                .fluid-centered {
                                    max-width: 100% !important;
                                    height: auto !important;
                                    margin-left: auto !important;
                                    margin-right: auto !important;
                                }
                                /* And center justify these ones. */
                                .fluid-centered {
                                    margin-left: auto !important;
                                    margin-right: auto !important;
                                }

                                /* What it does: Forces table cells into full-width rows. */
                                .stack-column,
                                .stack-column-center {
                                    display: block !important;
                                    width: 100% !important;
                                    max-width: 100% !important;
                                    direction: ltr !important;
                                }
                                /* And center justify these ones. */
                                .stack-column-center {
                                    text-align: center !important;
                                }

                                /* What it does: Generic utility class for centering. Useful for images, buttons, and nested tables. */
                                .center-on-narrow {
                                    text-align: center !important;
                                    display: block !important;
                                    margin-left: auto !important;
                                    margin-right: auto !important;
                                    float: none !important;
                                }
                                table.center-on-narrow {
                                    display: inline-block !important;
                                }

                            }

                        </style>
                        </head>
                        <body bgcolor='#e0e0e0' width='100%' style='margin 0;' yahoo='yahoo'>
                        <table bgcolor='#e0e0e0' cellpadding='0' cellspacing='0' border='0' height='100%' width='100%' style='border-collapsecollapse;'>
                          <tr>
                            <td><center style='width 100%;'>

                                <!-- Visually Hidden Preheader Text : BEGIN -->
                                <!-- Visually Hidden Preheader Text : END --> 

                                <!-- Email Header : BEGIN -->
                                <table align='center' width='600' class='email-container'>

                              </table>
                                <!-- Email Header : END --> 

                                <!-- Email Body : BEGIN -->
                                <table cellspacing='0' cellpadding='0' border='0' align='center' bgcolor='#ffffff' width='600' class='email-container'>

                    			<tr>
                                    <td style='padding:  5px 0; text-align: center'><a href='https://www.declaracioneside.com'><img src='https://www.declaracioneside.com/images/icons/logo1.png' width='200' height='70' alt='DeclaracionesIDE.com' border='0'></a></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <!-- Hero Image, Flush : BEGIN -->

                                <!-- Hero Image, Flush : END --> 

                                <!-- 1 Column Text : BEGIN -->
                                <tr>
                                    <td style='padding: 2px; text-align: center; font-family: sans-serif; font-size:  15px; mso-height-rule: exactly; line-height: 1px; color: #555555;'>
                                      <h2 style='font-family:Calibri; color:#306BBC'>" + IIf(institucion = "", "A quien corresponda", institucion) + ": </h2>
                    				  <table border='0' cellpadding='2px'>
                    					  <tbody>
                    						<tr>
                    						  <th><h1 style='color:darkblue;'>Cotización</h1></th>
                    							<td></td>	
                    						</tr>
                    					  </tbody>
                    					</table>



                                      <!-- Button : Begin -->                                

                                    <!-- Button : END --></td>
                                </tr>
                              </table>

                                <table cellspacing='0' cellpadding='0' border='0' align='center' bgcolor='#ffffff' width='600' class='email-container'>
                                  <!-- 1 Column Text : BEGIN --> 

                                  <!-- Background Image with Text : BEGIN -->
                                  <!-- Background Image with Text : END --> 


                                  <!-- Three Even Columns : BEGIN -->
                                  <tr>
                                    <td align='center' valign='top' style='border-spacing:  5px;padding: 5px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'>
                                      <tr >                    
                                        <td width='90%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>Declaraciones de depósitos en efectivo del 2022 en adelante</td>
                                          </tr>
                                        </table></td>                                        
                    					  <td width='10%' style='background-color: #306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>IVA incluido</td>
                                          </tr>

                                        </table></td>

                                      </tr>
                                      </table></td>
                                  </tr>

                    			<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Plataforma para Creación de mensuales CON DATOS por ejercicio(año) a declarar</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 4,500</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>
<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Envío de mensuales CON DATOS por ejercicio(año) a declarar</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 2,400</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>
<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Creación y envío de mensuales EN CEROS por ejercicio(año) a declarar</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 4,000</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>
<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Plataforma para Creación de mensuales CON DATOS por MES a declarar</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 375</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>
<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Envío de mensuales CON DATOS por MES a declarar</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 200</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>
<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Creación y envío de mensuales EN CEROS por MES a declarar</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 334</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>



<tr>
                                    <td align='center' valign='top' style='border-spacing:  5px;padding: 5px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'>
                                      <tr >                    
                                        <td width='90%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>Declaraciones de depósitos en efectivo previas al 2022</td>
                                          </tr>
                                        </table></td>                                        
                    					  <td width='10%' style='background-color: #306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>IVA incluido</td>
                                          </tr>

                                        </table></td>

                                      </tr>
                                      </table></td>
                                  </tr>

                    			<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='90%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Plataforma para Creación y envío por ejercicio(año) a declarar desde plataforma</strong></em></td>
                                          </tr>
                                        </table></td>
                    					  <td width='10%' class='stack-column-center'>
                                            <table cellspacing='0' cellpadding='0' border='0'>
                                              <tr >
                                                <td style='padding:  5px; text-align: center; '>$ 3,300</td>
                                              </tr>
                                            </table>
                                           </td>
                                      </tr>
                                      </table></td>
                                  </tr>






                                    <tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td colspan=4 style='background-color:#99FF99; color:black;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>o bien 15% MENOS de lo que pagas o tengas cotizado</td>
                                          </tr>
                                        </table></td>                                        
                                      </tr>
                                      </table></td>
                                  </tr>

                                  <!-- Three Even Columns : END --> 

                    				<tr style='background-color: darkblue; color: white; font-family: calibri;'>
                    					<td align='center' valign='top' style='border-spacing: 5px;padding: 3px;'>Formas de pago 
                    					</td>
                    				</tr>
                    			<tr>
                                    <td align='center' valign='top' style='border-spacing: 5px;padding: 0px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'>
                                      <tr >
                                        <td width='25%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>Beneficiario</td>
                                          </tr>
                                        </table></td>
                                        <td width='50%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>Banamex</td>
                                          </tr>

                                        </table></td>
                    					  <td width='25%' style='background-color: #306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr>
                                            <td style='padding: 5px; text-align: center'>FarmaciaGuadalajara,7Eleven</td>
                                          </tr>

                                        </table></td>


                                      </tr>
                                      </table></td>
                                  </tr>

                    			<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>
                                        <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>JOB JOSUE CONSTANTINO PRADO</td>
                                          </tr>
                                        </table></td>
                                        <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>Núm. de cuenta
                    7012000004874899 </td>
                                          </tr>

                                        </table></td>
                    					  <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>Núm. de CLABE
                    002470701248748996 </td>
                                          </tr>

                                        </table></td>
                    					  <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>Número de tarjeta
                    5204167339542094 </td>
                                          </tr>

                                        </table></td>


                                      </tr>
                                      </table></td>
                                  </tr>

                    			  <tr style='background-color: steelblue; color: white; font-family: calibri;'>
                    					<td align='center' valign='top' style='border-spacing: 5px;padding: 3px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'> Consideraciones: </table>
                    					</td>
                    				</tr>
                    				<tr style='font-family: calibri;'>
                    					<td align='justify' valign='top' style='border-spacing: 0px;padding: 5px;font-size: 9pt; font-family:Calibri;'>✓ <strong>Declaraciones MENSUALES IDE OBLIGATORIAS en 2022 y 2023</strong>, fundamento legal Ley del Impuesto sobre la Renta para 2022: Artículo 55. fracción IV , y Resolución Miscelánea Fiscal para 2022: Reglas 3.5.19., 3.5.21., Artículo TRIGÉSIMO SEGUNDO transitorio y Artículo CUARTO transitorio de la Novena Resolución de Modificaciones a la Resolución Miscelánea Fiscal para 2022. <br>
✓ Adjuntamos cotización en PDF<br>
✓ Contrata nuestro servicio de Declaracion Anual de INTERESES <a href='https://www.intereses.facturaselectronicascfdi.com/' target='_blank'>aqui </a> y el timbrado de los CFDI de retenciones correspondientes<br>
                    					  ✓ Contáctanos para hacerte contrato electrónico con estos precios<br>
                    ✓ Envía comprobante de pago al correo declaracioneside@gmail.com para activarte el contrato, indicando datos de facturación: RFC, razon social, forma y metodo de
                    pago, uso del comprobante; solicita tu factura en máximo 3 días una vez hecho el pago<br>
                    ✓ Precios en pesos mexicanos. Cotizacion vigente por 30 dias, te mejoramos cualquier cotizacion. Soporte sin costo L-V de 10am-5pm<br>
                    ✓ Al pagar especifica como referencia el número de esta cotización <br>
                    ✓Para pagos en línea con tarjeta de crédito, de debito o paypal, en el menu: Cuenta➝mis contratos➝seleccione el contrato a pagar➝pagar con tarjeta (si el pago es en linea agregar 3.95% + $4 + iva)<br>
                    ✓ Contrata la cantidad de declaraciones que requieras en ceros o con datos<br>
                    ✓Para declaraciones con datos, puedes reimportar&nbsp;desde excel la declaración&nbsp;cuantas veces necesites para cualquier corrección&nbsp;sin costo adicional&nbsp;antes de enviarla. El formato para declarar con datos puedes solicitarlo via correo o por telefono al contratar nuestros servicios<br>
                    ✓ Presentamos tu declaración contratando el servicio correspondiente, sube los archivos de las declaraciones puntualmente para nosotros enviar tus declaraciones, Puedes subir la FIEL, o nos conectamos cada mes a un equipo donde la tengas <br>
                    ✓ Los acuses se generan en un periodo de 2-24 hrs hábiles<br>
                    ✓ Formato 2022 en  ✓ <a href='https://www.declaracioneside.com/ejemploMensual22.xlsx'> https://www.declaracioneside.com/ejemploMensual22.xlsx </a><br>
                    ✓ El plazo para presentar Enero a Noviembre es Diciembre 2022 del dia 1 al último <br>
                    ✓ <a href='https://www.declaracioneside.com/videoman.aspx'>Video tutoriales</a> <br><br>

                    					<table cellspacing='0' cellpadding='0' border='0' align='center' style='margin: auto'>
                                        <tr>
                                        <td style='border-radius:  3px; background: #222222; text-align: center;' class='button-td'><a href='https://www.declaracioneside.com/registro.aspx' style='background: #222222; border: 15px solid #222222; padding: 0 10px;color: #ffffff; font-family: sans-serif; font-size:  13px; line-height: 1.1; text-align: center; text-decoration: none; display: block; border-radius: 3px; font-weight: bold;' class='button-a'> 
                                          <!--[if mso]>&nbsp;&nbsp;&nbsp;&nbsp;<![endif]-->Registrarme ahora<!--[if mso]>&nbsp;&nbsp;&nbsp;&nbsp;<![endif]--> 
                                          </a></td>
                                      </tr>
                                      </table>							
                    					<br>
                    					</td>
                    					</tr>
                    				<tr style='font-family: calibri;'>
                    					<td align='center' valign='top' style='border-spacing: 0px;padding: 3px;'>
                    					Página <font color='#0000ff'><u><a href='https://www.declaracioneside.com'>www.declaracioneside.com</a></u></font><br>
                    					Correo: <a href='mailto:declaracioneside@gmail.com'>declaracioneside@gmail.com</a><br>
                    					Tels. 4436903616, 4432180237<br><br>

                    					<a style='text-decorationunderline;' href='https://www.declaracioneside.com/unsuscribe.aspx'>
                    					<unsubscribe style='color:#888888; text-decoration:underline;'>Darme de baja de lista de correos</unsubscribe>
                    					</a>						
                    						<br><br>
                    					</td>
                    				</tr>

                                </table>
                              </center></td>
                          </tr>
                        </table>
                    </body>
                    </html>
                    "
                    elcorreo.IsBodyHtml = True
                    elcorreo.Priority = System.Net.Mail.MailPriority.High
                    elcorreo.Attachments.Add(New System.Net.Mail.Attachment(destino))
                    Dim smpt As New System.Net.Mail.SmtpClient
                    smpt.Host = "smtp.gmail.com"
                    smpt.Port = "587"
                    smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
                    smpt.EnableSsl = True 'req p server gmail
                    Try
                        smpt.Send(elcorreo)
                    Catch ex As Exception
                        Response.Write("<script language='javascript'>alert('Error enviando correo de cotizacion IDE: " & ex.Message + ", intente mas tarde');</script>")
                    End Try

                    Session("barraIteracion") = Session("barraIteracion") + 1
                Next

                Session("barraIteracion") = Session("barraN")

                Return 1
            Else
                objThread.Abort()
                Return 0
            End If
        Catch ex As Exception
            If Not w Is Nothing Then
                w.Close(False)   'cierro excel y trabajo con la var
            End If
            If Not excel Is Nothing Then
                excel.Quit()
            End If
            w = Nothing
            excel = Nothing
            Session("error") = ex.Message
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
            statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
            objThread.Abort()
            Return 0
        Finally
            If Not w Is Nothing Then
                w.Close(False)   'cierro excel y trabajo con la var
            End If
            If Not excel Is Nothing Then
                excel.Quit()
            End If
            w = Nothing
            excel = Nothing
        End Try

    End Function

    Private Function cargarCot() As Integer
        Dim objThread As Thread = CType(Session("Thread"), Thread)
        Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
        Dim w As Workbook

        Try
            w = excel.Workbooks.Open(savePath)
            Dim sheet As Worksheet = w.Sheets(1) 'i     'abrirá la 1er hoja del libro
            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row 'sin rens en bco

            Session("barraN") = nRensPre

            w.Close(False)   'cierro excel y trabajo con la var
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then
                Dim rens As Integer = nRensPre 'array.GetUpperBound(0)
                Dim fecha, correo, institucion, num, atn, tel, pu, cant, subt, iva, tot, ultEjer, pends, correoLogin, pu2, cant2, subt2, iva2, tot2
                Dim q
                fecha = Now.ToString("dd-MM-yyyy_HH-mm-ss")
                Dim dirDestino = "C:\SAT\AUTOCOT\" + fecha
                cant = "1"
                cant2 = "1"
                Dim oDir As New System.IO.DirectoryInfo(Server.MapPath("~"))
                oDir.Attributes = oDir.Attributes And Not IO.FileAttributes.ReadOnly
                Dim origen As String = Server.MapPath("~/COTfmto2020.pdf")
                If (Not System.IO.Directory.Exists(dirDestino)) Then
                    System.IO.Directory.CreateDirectory(dirDestino)
                End If
                For ren As Integer = 2 To rens '1rens=encab 2o=datos
                    If Not array(ren, 1) Is Nothing Then
                        correo = array(ren, 1).ToString.ToUpper.Trim.Replace("'", "''")
                        correoLogin = correo
                    Else
                        Continue For
                    End If
                    If Not array(ren, 5) Is Nothing Then 'fac correos
                        If array(ren, 5) <> "NULL" Then
                            correo = correo + "," + array(ren, 5).ToString.ToUpper.Trim.Replace("'", "''")
                        End If
                    End If
                    If Not array(ren, 6) Is Nothing Then 'otrosCorreos
                        If array(ren, 6) <> "NULL" Then
                            correo = correo + "," + array(ren, 6).ToString.ToUpper.Trim.Replace("'", "''")
                        End If
                    End If
                    If Not array(ren, 2) Is Nothing Then
                        institucion = array(ren, 2).ToString.ToUpper.Trim.Replace("'", "''")
                    End If
                    If Not array(ren, 3) Is Nothing Then
                        tel = array(ren, 3).ToString.ToUpper.Trim.Replace("'", "''")
                    End If
                    If Not array(ren, 4) Is Nothing Then
                        atn = array(ren, 4).ToString.ToUpper.Trim.Replace("'", "''")
                    End If
                    If Not array(ren, 16) Is Nothing Then
                        ultEjer = array(ren, 16).ToString.ToUpper.Trim.Replace("'", "''")
                    End If
                    If Not array(ren, 10) Is Nothing Then
                        pu = array(ren, 10).ToString.ToUpper.Trim.Replace("'", "''")
                        subt = CDbl(pu) * CDbl(cant)
                        iva = subt * 0.16
                        tot = subt + iva
                    End If
                    If Not array(ren, 19) Is Nothing Then
                        pends = array(ren, 19).ToString.ToUpper.Trim.Replace("'", "''")
                    End If
                    If Not array(ren, 22) Is Nothing Then
                        pu2 = array(ren, 22).ToString.ToUpper.Trim.Replace("'", "''")
                        subt2 = CDbl(pu2) * CDbl(cant2)
                        iva2 = subt2 * 0.16
                        tot2 = subt2 + iva2
                    End If
                    num = "IDE " + fecha + " " + (ren - 1).ToString

                    'crear copia pdf
                    Dim destino As String = dirDestino + "\" + correoLogin + " " + num + ".pdf"
                    If File.Exists(destino) Then
                        File.Delete(destino)
                    End If

                    Dim pdfDoc As PdfDocument = New PdfDocument(New PdfReader(origen), New PdfWriter(destino))
                    Dim Form As PdfAcroForm = PdfAcroForm.GetAcroForm(pdfDoc, True)
                    Form.SetGenerateAppearance(True)
                    Dim font As PdfFont = PdfFontFactory.CreateFont(Server.MapPath("~/Calibri.ttf"), PdfEncodings.WINANSI)
                    Dim fontBold As PdfFont = PdfFontFactory.CreateFont(Server.MapPath("~/CalibriBold.ttf"), PdfEncodings.WINANSI)
                    Form.GetField("atn").SetValue(atn, font, 11.0F)
                    Form.GetField("correo").SetValue(correo, font, 8.0F)
                    Form.GetField("tel").SetValue(tel, font, 11.0F)
                    Form.GetField("institucion").SetValue(institucion, fontBold, 12.0F)
                    Form.GetField("fecha").SetValue(Left(fecha, 10), font, 11.0F)
                    Form.GetField("num").SetValue(num, font, 11.0F)
                    Form.GetField("pu").SetValue(FormatCurrency(pu, 2), font, 10.0F)
                    Form.GetField("cant").SetValue(cant, font, 10.0F)
                    Form.GetField("sub").SetValue(FormatCurrency(subt, 2), font, 10.0F)
                    Form.GetField("tot").SetValue(FormatCurrency(tot, 2), font, 10.0F)
                    Form.GetField("iva").SetValue(FormatCurrency(iva, 2), font, 10.0F)
                    Form.GetField("ultEjerc").SetValue(ultEjer, fontBold, 11.0F)
                    Form.GetField("pends").SetValue(pends, fontBold, 11.0F)
                    Dim adicionalDatos = ""
                    If Val(tot2) = 0 Then
                        adicionalDatos = "El envío en $0, corresponde a declaraciones sin datos a reportar. Si su declaración es con datos, sumar envío de $2400 por año, y un adicional en la creación de declaraciones, contáctanos."
                    End If
                    Form.GetField("adicionalDatos").SetValue(adicionalDatos, font, 9.0F)
                    Form.GetField("pu2").SetValue(FormatCurrency(pu2, 2), font, 10.0F)
                    Form.GetField("cant2").SetValue(cant2, font, 10.0F)
                    Form.GetField("sub2").SetValue(FormatCurrency(subt2, 2), font, 10.0F)
                    Form.GetField("tot2").SetValue(FormatCurrency(tot2, 2), font, 10.0F)
                    Form.GetField("iva2").SetValue(FormatCurrency(iva2, 2), font, 10.0F)

                    Form.FlattenFields()
                    pdfDoc.Close()
                    'abrimos el pdf
                    'Process.Start("acrord32.exe", destino)

                    oDir.Attributes = oDir.Attributes And IO.FileAttributes.ReadOnly


                    'enviar correo
                    Dim elcorreo As New System.Net.Mail.MailMessage
                    elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                    'elcorreo.To.Add(correo)
                    elcorreo.Bcc.Add(correo)
                    elcorreo.Subject = "Cotización de Declaración de Depósitos en Efectivo >= 2022"
                    elcorreo.Body = "
                    <html>
                        <head>
                        <meta charset='utf-8'>
                        <!-- utf-8 works for most cases -->
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <!-- Forcing initial-scale shouldn't be necessary -->
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <!-- Use the latest (edge) version of IE rendering engine -->
                        <title>EmailTemplate-Responsive</title>
                        <!-- The title tag shows in email notifications, like Android 4.4. -->
                        <!-- Please use an inliner tool to convert all CSS to inline as inpage or external CSS is removed by email clients -->
                        <!-- important in CSS is used to prevent the styles of currently inline CSS from overriding the ones mentioned in media queries when corresponding screen sizes are encountered -->

                        <!-- CSS Reset -->
                        <style type='text/css'>
                    /* What it does: Remove spaces around the email design added by some email clients. */
                          /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */
                    html,  body {
                    	margin: 0 !important;
                    	padding: 0 !important;
                    	height: 100% !important;
                    	width: 100% !important;
                    }
                    /* What it does: Stops email clients resizing small text. */
                    * {
                    	-ms-text-size-adjust: 100%;
                    	-webkit-text-size-adjust: 100%;
                    }
                    /* What it does: Forces Outlook.com to display emails full width. */
                    .ExternalClass {
                    	width: 100%;
                    }
                    /* What is does: Centers email on Android 4.4 */
                    div[style*='margin 16px 0'] {
                    	margin: 0 !important;
                    }
                    /* What it does: Stops Outlook from adding extra spacing to tables. */
                    table,  td {
                    	mso-table-lspace: 0pt !important;
                    	mso-table-rspace: 0pt !important;
                    }
                    /* What it does: Fixes webkit padding issue. Fix for Yahoo mail table alignment bug. Applies table-layout to the first 2 tables then removes for anything nested deeper. */
                    table {
                    	border-spacing: 0 !important;
                    	border-collapse: collapse !important;
                    	table-layout: fixed !important;
                    	margin: 0 auto !important;
                    }
                    table table table {
                    	table-layout: auto;
                    }
                    /* What it does: Uses a better rendering method when resizing images in IE. */
                    img {
                    	-ms-interpolation-mode: bicubic;
                    }
                    /* What it does: Overrides styles added when Yahoo's auto-senses a link. */
                    .yshortcuts a {
                    	border-bottom: none !important;
                    }
                    /* What it does: Another work-around for iOS meddling in triggered links. */
                    a[x-apple-data-detectors] {
                    	color: inherit !important;
                    }
                    </style>

                        <!-- Progressive Enhancements -->
                        <style type='text/css'>

                            /* What it does: Hover styles for buttons */
                            .button-td,
                            .button-a {
                                transition: all 100ms ease-in;
                            }
                            .button-td:hover,
                            .button-a:hover {
                                background: #555555 !important;
                                border-color: #555555 !important;
                            }

                            /* Media Queries */
                            @media screen and (max-width: 600px) {

                                .email-container {
                                    width: 100% !important;
                                }

                                /* What it does: Forces elements to resize to the full width of their container. Useful for resizing images beyond their max-width. */
                                .fluid,
                                .fluid-centered {
                                    max-width: 100% !important;
                                    height: auto !important;
                                    margin-left: auto !important;
                                    margin-right: auto !important;
                                }
                                /* And center justify these ones. */
                                .fluid-centered {
                                    margin-left: auto !important;
                                    margin-right: auto !important;
                                }

                                /* What it does: Forces table cells into full-width rows. */
                                .stack-column,
                                .stack-column-center {
                                    display: block !important;
                                    width: 100% !important;
                                    max-width: 100% !important;
                                    direction: ltr !important;
                                }
                                /* And center justify these ones. */
                                .stack-column-center {
                                    text-align: center !important;
                                }

                                /* What it does: Generic utility class for centering. Useful for images, buttons, and nested tables. */
                                .center-on-narrow {
                                    text-align: center !important;
                                    display: block !important;
                                    margin-left: auto !important;
                                    margin-right: auto !important;
                                    float: none !important;
                                }
                                table.center-on-narrow {
                                    display: inline-block !important;
                                }

                            }

                        </style>
                        </head>
                        <body bgcolor='#e0e0e0' width='100%' style='margin 0;' yahoo='yahoo'>
                        <table bgcolor='#e0e0e0' cellpadding='0' cellspacing='0' border='0' height='100%' width='100%' style='border-collapsecollapse;'>
                          <tr>
                            <td><center style='width 100%;'>

                                <!-- Visually Hidden Preheader Text : BEGIN -->
                                <!-- Visually Hidden Preheader Text : END --> 

                                <!-- Email Header : BEGIN -->
                                <table align='center' width='600' class='email-container'>

                              </table>
                                <!-- Email Header : END --> 

                                <!-- Email Body : BEGIN -->
                                <table cellspacing='0' cellpadding='0' border='0' align='center' bgcolor='#ffffff' width='600' class='email-container'>

                    			<tr>
                                    <td style='padding:  5px 0; text-align: center'><a href='https://www.declaracioneside.com'><img src='https://www.declaracioneside.com/images/icons/logo1.png' width='200' height='70' alt='alt_text' border='0'></a></td>
                                </tr>

                                <!-- Hero Image, Flush : BEGIN -->

                                <!-- Hero Image, Flush : END --> 

                                <!-- 1 Column Text : BEGIN -->
                                <tr>
                                    <td style='padding: 2px; text-align: center; font-family: sans-serif; font-size:  15px; mso-height-rule: exactly; line-height: 1px; color: #555555;'>
                                      <h2 style='font-family:Calibri; color:#306BBC'>Hola " + institucion + "</h2>
                    				  <table border='0' cellpadding='2px'>
                    					  <tbody>
                    						<tr>
                    						  <th><h1 style='color:darkblue;'>Cotización</h1></th>
                    							<td style='font-size: 11pt; font-family:Calibri; color: black'> Núm. " + num + "</td>	
                    						</tr>
                    					  </tbody>
                    					</table>



                                      <!-- Button : Begin -->                                

                                    <!-- Button : END --></td>
                                </tr>
                              </table>

                                <table cellspacing='0' cellpadding='0' border='0' align='center' bgcolor='#ffffff' width='600' class='email-container'>
                                  <!-- 1 Column Text : BEGIN --> 

                                  <!-- Background Image with Text : BEGIN -->
                                  <!-- Background Image with Text : END --> 


                                  <!-- Three Even Columns : BEGIN -->
                                  <tr>
                                    <td align='center' valign='top' style='border-spacing:  5px;padding: 5px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'>
                                      <tr >                    
                                        <td width='58.66%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>Concepto</td>
                                          </tr>
                                        </table></td>
                                        <td width='8%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>Cant.</td>
                                          </tr>

                                        </table></td>
                    					  <td width='16.66%' style='background-color: #306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr>
                                            <td style='padding: 5px; text-align: center'>Subtotal</td>
                                          </tr>

                                        </table></td>
                    					  <td width='16.66%' style='background-color: #306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>Total</td>
                                          </tr>

                                        </table></td>

                                      </tr>
                                      </table></td>
                                  </tr>

                    			<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='58.66%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Creación de 12 Declaraciones Mensuales de depósitos en efectivo</strong></em></td>
                                          </tr>
                                        </table></td>
                                        <td width='8%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>" + cant.ToString + "</td>
                                          </tr>

                                        </table></td>
                    					  <td width='16.66%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>" + FormatCurrency(subt, 2) + "</td>
                                          </tr>

                                        </table></td>
                    					  <td width='16.66%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>" + FormatCurrency(tot, 2) + "</td>
                                          </tr>

                                        </table></td>

                                      </tr>
                                      </table></td>
                                  </tr>


                                    <tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>                   
                                        <td width='58.66%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'><em><strong>Envío de 12 Declaraciones Mensuales de depósitos en efectivo</strong></em></td>
                                          </tr>
                                        </table></td>
                                        <td width='8%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>" + cant2.ToString + "</td>
                                          </tr>

                                        </table></td>
                    					  <td width='16.66%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>" + FormatCurrency(subt2, 2) + "</td>
                                          </tr>

                                        </table></td>
                    					  <td width='16.66%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>" + FormatCurrency(tot2, 2) + "</td>
                                          </tr>

                                        </table></td>

                                      </tr>
                                      </table></td>
                                  </tr>


                                  <!-- Three Even Columns : END --> 

                    				<tr style='background-color: darkblue; color: white; font-family: calibri;'>
                    					<td align='center' valign='top' style='border-spacing: 5px;padding: 3px;'>Formas de pago 
                    					</td>
                    				</tr>
                    			<tr>
                                    <td align='center' valign='top' style='border-spacing: 5px;padding: 0px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'>
                                      <tr >
                                        <td width='25%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>Beneficiario</td>
                                          </tr>
                                        </table></td>
                                        <td width='50%' style='background-color:#306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>Banamex</td>
                                          </tr>

                                        </table></td>
                    					  <td width='25%' style='background-color: #306BBC; color:#FFFFFF;'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr>
                                            <td style='padding: 5px; text-align: center'>FarmaciasGuadalajara,7Eleven</td>
                                          </tr>

                                        </table></td>


                                      </tr>
                                      </table></td>
                                  </tr>

                    			<tr style='font-size:  10pt; font-family:Calibri'>
                                    <td align='center' valign='top' style='padding: 0px;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                      <tr>
                                        <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center;'>JOB JOSUE CONSTANTINO PRADO</td>
                                          </tr>
                                        </table></td>
                                        <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding: 5px; text-align: center'>Núm. de cuenta
                    7012000004874899 </td>
                                          </tr>

                                        </table></td>
                    					  <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>Núm. de CLABE
                    002470701248748996 </td>
                                          </tr>

                                        </table></td>
                    					  <td width='25%' class='stack-column-center'><table cellspacing='0' cellpadding='0' border='0'>
                                          <tr >
                                            <td style='padding:  5px; text-align: center'>Número de tarjeta
                    5204167339542094 </td>
                                          </tr>

                                        </table></td>


                                      </tr>
                                      </table></td>
                                  </tr>

                    			  <tr style='background-color: steelblue; color: white; font-family: calibri;'>
                    					<td align='center' valign='top' style='border-spacing: 5px;padding: 3px;'><table cellspacing='2' cellpadding='0' border='0' width='100%'> Consideraciones: </table>
                    					</td>
                    				</tr>
                    				<tr style='font-family: calibri;'>
                    					<td align='justify' valign='top' style='border-spacing: 0px;padding: 5px;font-size: 9pt; font-family:Calibri;'>✓  Fundamento legal: ISR Art. 55 Fracc IV, declaraciones <strong>MENSUALES de depositos en efectivo 2022 OBLIGATORIAS</strong>. El formato para las mensules esta <a href='https://www.declaracioneside.com/ejemploMensual22.xlsx' target='_blank'> aqui </a> El <strong>plazo para presentar Enero a Noviembre 2022 es en Diciembre 2022</strong> del dia 1 al ultimo de Diciembre 2022. Las anuales del 2022 se derogaron<br>
                                            ✓ Adjuntamos cotización en PDF<br>
                                            " + adicionalDatos + "<br>
                                            ✓ Contrata nuestro servicio de Declaracion Anual de INTERESES <a href='https://www.intereses.facturaselectronicascfdi.com/' target='_blank'>aqui </a> y el timbrado de los CFDI de retenciones correspondientes<br>
                                            ✓ Contrata nuestro servicio de timbrado de ESTADOS DE CUENTA <a href='https://facturaselectronicascfdi.com/' target='_blank'>aqui </a><br>
                    					  ✓ Contáctanos para hacerte contrato electrónico con estos precios<br>
                    ✓ Envía comprobante de pago al correo declaracioneside@gmail.com para activarte el contrato, indicando datos de facturación: RFC, razon social, forma y metodo de
                    pago, uso del comprobante; solicita tu factura en máximo 3 días una vez hecho el pago<br>
                    ✓ Precios en pesos mexicanos. Cotizacion vigente por 30 dias, te mejoramos cualquier cotizacion. Soporte sin costo L-V de 10am-3pm<br>
                    ✓ Al pagar especifica como referencia el número de esta cotización <br>
                    ✓Para pagos en línea con tarjeta de crédito, de debito o paypal, en el menu: Cuenta➝mis contratos➝seleccione el contrato a pagar➝pagar con tarjeta (si el pago es en linea agregar 3.95% + $4 + iva)<br>
                    ✓ Contrata la cantidad de declaraciones que requieras en ceros o con datos para las mensuales<br>
                    ✓Para declaraciones con datos, puedes reimportar&nbsp;desde excel o xml la declaración&nbsp;cuantas veces necesites para cualquier corrección&nbsp;sin costo adicional&nbsp;antes de enviarla. El formato para declarar con datos puedes solicitarlo via correo o por telefono al contratar nuestros servicios<br>
                    ✓ Envía tu declaración o solicita que lo hagamos por ti contratando el servicio correspondiente, sube los archivos de las declaraciones puntualmente<br>
                    ✓ Para nosotros enviar tus declaraciones, Puedes subir la FIEL, o nos conectamos cada mes a un equipo donde la tengas <br>
                    ✓ Los acuses se generan en un periodo de 2-24 hrs hábiles<br>
                    ✓ <a href='https://www.declaracioneside.com/videoman.aspx'>Video tutoriales</a> <br><br>

                    					<table cellspacing='0' cellpadding='0' border='0' align='center' style='margin: auto'>
                                        <tr>
                                        <td style='border-radius:  3px; background: #222222; text-align: center;' class='button-td'><a href='https://declaracioneside.com' style='background: #222222; border: 15px solid #222222; padding: 0 10px;color: #ffffff; font-family: sans-serif; font-size:  13px; line-height: 1.1; text-align: center; text-decoration: none; display: block; border-radius: 3px; font-weight: bold;' class='button-a'> 
                                          <!--[if mso]>&nbsp;&nbsp;&nbsp;&nbsp;<![endif]-->Ir a mi cuenta<!--[if mso]>&nbsp;&nbsp;&nbsp;&nbsp;<![endif]--> 
                                          </a></td>
                                      </tr>
                                      </table>							
                    					<br>
                    					</td>
                    					</tr>
                    				<tr style='font-family: calibri;'>
                    					<td align='center' valign='top' style='border-spacing: 0px;padding: 3px;'>
                    					Página <font color='#0000ff'><u><a href='https://www.declaracioneside.com'>www.declaracioneside.com</a></u></font><br>
                    					Correo: <a href='mailto:declaracioneside@gmail.com'>declaracioneside@gmail.com</a><br>
                    					Tels. 4436903616, 4432180237<br>
                    									<a href='https://www.youtube.com/user/declaracioneside'><img src='https://www.declaracioneside.com/iconoyoutube.png' width='40' height='40'></a> <a href='https://twitter.com/inowebs'><img src='https://www.declaracioneside.com/twitter.jpg' width='40' height='40'></a> <a href='https://www.facebook.com/depositosenefectivo'><img src='https://www.declaracioneside.com/facebook.jpg' width='40' height='40'></a><br><br>

                    					<a style='text-decorationunderline;' href='mailto:declaracioneside@gmail.com?subject=Unsuscribe de correos de cotización " + correoLogin + "&body=Darme de baja de sus comunicados de correos'>
                    					<unsubscribe style='color:#888888; text-decoration:underline;'>Darme de baja de lista de correos</unsubscribe>
                    					</a>						
                    						<br><br>
                    					</td>
                    				</tr>

                                </table>
                              </center></td>
                          </tr>
                        </table>
                    </body>
                    </html>
                    "
                    elcorreo.IsBodyHtml = True
                    elcorreo.Priority = System.Net.Mail.MailPriority.High
                    elcorreo.Attachments.Add(New System.Net.Mail.Attachment(destino))
                    Dim smpt As New System.Net.Mail.SmtpClient
                    smpt.Host = "smtp.gmail.com"
                    smpt.Port = "587"
                    smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
                    smpt.EnableSsl = True 'req p server gmail
                    Try
                        smpt.Send(elcorreo)
                    Catch ex As Exception
                        Response.Write("<script language='javascript'>alert('Error enviando correo de cotizacion IDE: " & ex.Message + ", intente mas tarde');</script>")
                    End Try

                    Session("barraIteracion") = Session("barraIteracion") + 1
                Next

                Session("barraIteracion") = Session("barraN")

                Return 1
            Else
                objThread.Abort()
                Return 0
            End If
        Catch ex As Exception
            If Not w Is Nothing Then
                w.Close(False)   'cierro excel y trabajo con la var
            End If
            If Not excel Is Nothing Then
                excel.Quit()
            End If
            w = Nothing
            excel = Nothing
            Session("error") = ex.Message
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
            statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
            objThread.Abort()
            Return 0
        Finally
            If Not w Is Nothing Then
                w.Close(False)   'cierro excel y trabajo con la var
            End If
            If Not excel Is Nothing Then
                excel.Quit()
            End If
            w = Nothing
            excel = Nothing
        End Try

    End Function

    Protected Sub campos_Click(sender As Object, e As EventArgs) Handles campos.Click
        'Dim pdfTemplate As String = "C:\Users\Job\Source\Repos\IDE\IDE\WebApplication1\WebApplication1\COTfmto2020.pdf"
        'Dim pdfReader As PdfReader = New PdfReader(pdfTemplate)
        'Dim sb As New StringBuilder()

        'Dim de
        'For Each de In pdfReader.AcroFields.Fields
        '    sb.Append(de.Key.ToString() + Environment.NewLine)
        'Next
        'statusImport.Text = sb.ToString()


        'Dim File As FileInfo = New FileInfo("C:\Users\Job\Source\Repos\IDE\IDE\WebApplication1\WebApplication1\COTfmto2020JAJA.pdf")
        'File.Directory.Create()
        'Dim pdfDoc As PdfDocument = New PdfDocument(New PdfReader("C:\Users\Job\Source\Repos\IDE\IDE\WebApplication1\WebApplication1\COTfmto2020.pdf"), New PdfWriter("C:\Users\Job\Source\Repos\IDE\IDE\WebApplication1\WebApplication1\COTfmto2020JAJA.pdf"))
        'Dim Form As PdfAcroForm = PdfAcroForm.GetAcroForm(pdfDoc, True)
        'Form.SetGenerateAppearance(True)
        'Dim font As PdfFont = PdfFontFactory.CreateFont("C:\Users\Job\Source\Repos\IDE\IDE\WebApplication1\WebApplication1\Calibri.ttf", PdfEncodings.WINANSI)
        'Form.GetField("atn").SetValue("JOB JOSUE CONSTANTINO PRADO", font, 10.0F)
        'Form.GetField("correo").SetValue("job001@hotmail.com", font, 10.0F)
        'Form.GetField("institucion").SetValue("CAJA MIL CUMBRES SOCIEDAD COOPERATIVA DE AHORRO Y PRESTAMO", font, 10.0F)
        'Form.GetField("fecha").SetValue("2020-12-23", font, 10.0F)
        'Form.GetField("num").SetValue("IDE 2020-12-23_10-12-12", font, 10.0F)
        'Form.GetField("pu").SetValue("$1,000.00", font, 10.0F)
        'Form.GetField("cant").SetValue("1", font, 10.0F)
        'Form.GetField("sub").SetValue("$1,000.00", font, 10.0F)
        'Form.GetField("tot").SetValue("$1,116.00", font, 10.0F)
        'Form.GetField("iva").SetValue("$116.00", font, 10.0F)
        'Form.GetField("ultEjerc").SetValue("2018", font, 10.0F)
        'Form.GetField("pends").SetValue("2", font, 10.0F)
        'Form.FlattenFields()
        'pdfDoc.Close()


    End Sub

    Private Sub generico_Click(sender As Object, e As EventArgs) Handles generico.Click
        If subeArch("generico") < 1 Then
            Exit Sub
        End If

        Dim objThread As New Thread(New System.Threading.ThreadStart(AddressOf DoTheWork2))
        objThread.IsBackground = True
        objThread.Start()
        Session("Thread") = objThread

        Timer1.Enabled = True

        'DoTheWork()
    End Sub


End Class