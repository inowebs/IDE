Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm7
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader


    Protected Sub ingresar_Click(sender As Object, e As EventArgs) Handles ingresar.Click
        myCommand = New SqlCommand("SELECT id FROM admin WHERE nombre='" + pass1.Text.Trim + "'", myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.HasRows Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('Contraseña incorrecta');</script>")
            btnOculto_Click(sender, e)
        Else
            dr.Close()
            Session("admonIn") = "1"
            'UpdatePanel3.Update()
            panel1_ModalPopupExtender.Hide()
        End If

    End Sub
    Protected Sub btnOculto_Click(sender As Object, e As EventArgs) Handles btnOculto.Click
        '                UpdatePanel3.Update()
        panel1_ModalPopupExtender.Show()
        Panel2.Style.Remove("display")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ToolkitScriptManager1.RegisterPostBackControl(ingresar)
        ToolkitScriptManager1.RegisterPostBackControl(btnOculto)
        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("lan")) Then
                If Request.QueryString("lan") = "1" Then
                    Session("runAsAdmin") = "1"
                End If
            End If
            If Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1" Or Left(Request.ServerVariables("REMOTE_ADDR"), 3) = "::1" Or Left(Request.ServerVariables("REMOTE_ADDR"), 9) = "localhost" Or Session("runAsAdmin") = "1" Then 'red local
                btnOculto_Click(sender, e)
            Else
                Response.Write("<script language='javascript'>alert('Acceso denegado por su ubicación/forma de acceso');</script>")
                Response.Write("<script>location.href='Login.aspx';</script>")
            End If
        End If
        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat dmy", myConnection)
        myCommand.ExecuteNonQuery()

        If Session("runAsAdmin") = "1" Then
            add.Visible = False
        End If

        GridView3.SelectedIndex = -1

        ' Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll.ClientID + "');", True)
    End Sub

    Private Function validar() As Integer
        If chkRazon.Checked = True And razon.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la institución');</script>")
            razon.Focus()
            Return 0
        End If

        If chkuuid.Checked = True And uuid.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Especifique uuid');</script>")
            uuid.Focus()
            Return 0
        End If

        If chkCorreo.Checked = True And correo.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            correo.Focus()
            Return 0
        End If
        If chkFechaPago.Checked = True Then
            If fechaPago.Text.Trim = "" Or fechaPago2.Text.Trim = "" Then
                Response.Write("<script language='javascript'>alert('Especifique la fecha de pago');</script>")
                fechaPago.Focus()
                Return 0
            Else
                Dim dtnow As DateTime
                Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
                If regDate.IsMatch(fechaPago.Text.Trim) Then
                    If Not DateTime.TryParse(fechaPago.Text.Trim, dtnow) Then
                        fechaPago.Focus()
                        Response.Write("<script language='javascript'>alert('fechaPago fecha invalida');</script>")
                        Return 0
                    End If
                Else
                    fechaPago.Focus()
                    Response.Write("<script language='javascript'>alert('fechaPago formato de fecha no valido (dd/mm/aaaa)');</script>")
                    Return 0
                End If
                If regDate.IsMatch(fechaPago2.Text.Trim) Then
                    If Not DateTime.TryParse(fechaPago2.Text.Trim, dtnow) Then
                        fechaPago2.Focus()
                        Response.Write("<script language='javascript'>alert('fechaPago2 fecha invalida');</script>")
                        Return 0
                    End If
                Else
                    fechaPago2.Focus()
                    Response.Write("<script language='javascript'>alert('fechaPago2 formato de fecha no valido (dd/mm/aaaa)');</script>")
                    Return 0
                End If

            End If
        End If

        If chkFechaContra.Checked = True Then
            If fechaContra1.Text.Trim = "" Or fechaContra2.Text.Trim = "" Then
                Response.Write("<script language='javascript'>alert('Especifique la fecha del contrato');</script>")
                fechaContra1.Focus()
                Return 0
            Else
                Dim dtnow As DateTime
                Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
                If regDate.IsMatch(fechaContra1.Text.Trim) Then
                    If Not DateTime.TryParse(fechaContra1.Text.Trim, dtnow) Then
                        fechaContra1.Focus()
                        Response.Write("<script language='javascript'>alert('fechaContrato fecha invalida');</script>")
                        Return 0
                    End If
                Else
                    fechaContra1.Focus()
                    Response.Write("<script language='javascript'>alert('fechaPago formato de fecha no valido (dd/mm/aaaa)');</script>")
                    Return 0
                End If
                If regDate.IsMatch(fechaContra2.Text.Trim) Then
                    If Not DateTime.TryParse(fechaContra2.Text.Trim, dtnow) Then
                        fechaContra2.Focus()
                        Response.Write("<script language='javascript'>alert('fechaContrato2 fecha invalida');</script>")
                        Return 0
                    End If
                Else
                    fechaContra2.Focus()
                    Response.Write("<script language='javascript'>alert('fechaContra2 formato de fecha no valido (dd/mm/aaaa)');</script>")
                    Return 0
                End If

            End If
        End If

        If chkMonto.Checked = True And (monto1.Text.Trim = "" Or monto2.Text.Trim = "") Then
            Response.Write("<script language='javascript'>alert('Falta algun monto');</script>")
            monto1.Focus()
            Return 0
        End If

        Return 1
    End Function


    Protected Sub buscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Buscar.Click
        If validar() < 1 Then
            Exit Sub
        End If
        Dim q
        SqlDataSource3.SelectCommand = "SELECT co.id,co.precioNetoContrato,co.fechaPago,pla.elplan,co.fecha,co.nDeclHechas,co.nDeclContratadas,cli.correo,co.esRegularizacion, co.uuid,co.pagoRealizado FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id "
        q = "SELECT SUM(co.precioNetoContrato) as total FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id "
        If chkRazon.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND cli.razonSoc LIKE '%" + razon.Text.ToUpper.Trim + "%'"
            q = q + " AND cli.razonSoc LIKE '%" + razon.Text.ToUpper.Trim + "%'"
        End If
        If chkNum.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.id=" + num.Text.Trim
            q = q + " AND co.id=" + num.Text.Trim
        End If
        If chkCorreo.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND cli.correo='" + correo.Text.ToUpper.Trim + "'"
            q = q + " AND cli.correo='" + correo.Text.ToUpper.Trim + "'"
        End If
        If chkMonto.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.precioNetoContrato>=" + monto1.Text.ToUpper.Trim + " AND co.precioNetoContrato<=" + monto2.Text.ToUpper.Trim
            q = q + " AND co.precioNetoContrato>=" + monto1.Text.ToUpper.Trim + " AND co.precioNetoContrato<=" + monto2.Text.ToUpper.Trim
        End If
        If chkuuid.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.uuid='" + uuid.Text.ToUpper.Trim + "'"
            q = q + " AND co.uuid='" + uuid.Text.ToUpper.Trim + "'"
        End If
        If chkFormapago.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.pagoRealizado='" + pagoRealizado.SelectedItem.Text + "'"
            q = q + " AND co.pagoRealizado='" + pagoRealizado.SelectedItem.Text + "'"
        End If
        If chkPagado.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.fechaPago is not null"
            q = q + " AND co.fechaPago is not null"
        End If
        If chkFechaPago.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.fechaPago>='" + Format(Convert.ToDateTime(fechaPago.Text.Trim), "dd-MM-yyyy") + "' AND co.fechaPago<='" + Format(Convert.ToDateTime(fechaPago2.Text.Trim), "dd-MM-yyyy") + "'"
            q = q + " AND co.fechaPago >= '" + Format(Convert.ToDateTime(CDate(fechaPago.Text.Trim)), "dd-MM-yyyy") + "' AND co.fechaPago <= '" + Format(Convert.ToDateTime(CDate(fechaPago2.Text.Trim)), "dd-MM-yyyy") + "'"
        End If
        If chkFechaContra.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.fecha>='" + Format(Convert.ToDateTime(fechaContra1.Text.Trim), "dd-MM-yyyy") + "' AND co.fecha<='" + Format(Convert.ToDateTime(fechaContra2.Text.Trim), "dd-MM-yyyy") + "'"
            q = q + " AND co.fecha >= '" + Format(Convert.ToDateTime(CDate(fechaContra1.Text.Trim)), "dd-MM-yyyy") + "' AND co.fecha <= '" + Format(Convert.ToDateTime(CDate(fechaContra2.Text.Trim)), "dd-MM-yyyy") + "'"
        End If
        If chkPlan.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND pla.elplan='" + elPlan.Text + "'"
            q = q + " AND pla.elplan='" + elPlan.Text + "'"
        End If
        If chkStatus.Checked = True Then
            Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
            fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
            If Status.Text = "VIGENTES" Then
                SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan='BASICO' OR pla.elplan='CEROS')) or ('" + Format(Convert.ToDateTime(fechaUltima), "dd-MM-yyyy") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) )"
                q = q + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan='BASICO' OR pla.elplan='CEROS')) or ('" + Format(Convert.ToDateTime(fechaUltima), "dd-MM-yyyy") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) )"
            Else 'vencidos
                SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND ( (nDeclHechas >= nDeclContratadas and (pla.elplan='BASICO' OR pla.elplan='CEROS')) or ('" + Format(Convert.ToDateTime(fechaUltima), "dd-MM-yyyy") + "' > fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) )"
                q = q + " AND ( (nDeclHechas >= nDeclContratadas and (pla.elplan='BASICO' OR pla.elplan='CEROS')) or ('" + Format(Convert.ToDateTime(fechaUltima), "dd-MM-yyyy") + "' > fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) )"
            End If
        End If
        If chkReg.Checked = True Then
            If Reg.Text = "CON" Then
                SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.esRegularizacion=1"
                q = q + " AND co.esRegularizacion=1"
            Else 'vencidos
                SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.esRegularizacion=0"
                q = q + " AND co.esRegularizacion=0"
            End If
        End If
        If chkIni.Checked = True Then
            SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " AND co.esEl1o=1"
            q = q + " AND co.esEl1o=1"
        End If

        SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " ORDER BY co.id desc"
        GridView3.DataBind()

        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        Dim total
        If IsDBNull(dr("total")) = True Then
            total = "0"
        Else
            total = FormatCurrency(dr("total")).ToString
        End If
        nRegs.Text = FormatNumber(GridView3.Rows.Count.ToString, 0) + " Registros, PrecioNetoTotal = " + total
        dr.Close()
    End Sub

    Private Sub webform7_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'myConnection.Close()
    End Sub

    Protected Sub GridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView3.SelectedIndexChanged
        Dim row As GridViewRow = GridView3.SelectedRow
        Session("GidContrato") = row.Cells(1).Text

        Dim q2
        q2 = "SELECT idCliente FROM contratos WHERE id=" + row.Cells(1).Text
        myCommand = New SqlCommand(q2, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        Session("GidCliente") = dr("idCliente")
        dr.Close()

        Response.Redirect("~/contrato.aspx")
    End Sub

    Protected Sub add_Click(sender As Object, e As EventArgs) Handles add.Click
        session("GidContrato") = Nothing
        Response.Redirect("~/contrato.aspx")
    End Sub

    Protected Sub chkRazon_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRazon.CheckedChanged

    End Sub
End Class