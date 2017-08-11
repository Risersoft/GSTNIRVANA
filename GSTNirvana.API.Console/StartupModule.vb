Imports risersoft.shared.cloud
Imports risersoft.shared.portable.Models.Gst

Module StartupModule

    Sub Main()


        Dim provider As New clsClientCredTokenProvider("4AFKA8#EDAXA", "753RPM", "http://login.risersoft.com", True)
        Dim token = provider.AuthenticateAsync.Result
        Dim client As New WebApiClientToken(provider._clientId, provider.TokenResponse.AccessToken)

        Dim invoice As New GstInvoiceInfo
        invoice.INUM = "Test1"
        invoice.IDT = Now.Date
        invoice.CTIN = "33GSPXXXXXX1ZA"
        invoice.GSTInvoiceType = "b2b"
        invoice.Sply_Ty = "inter"
        invoice.CampusID = 1
        invoice.inv_typ = "R"
        invoice.DocType = "IS"
        invoice.InvoiceItems = New List(Of GstInvoiceItemInfo)
        invoice.InvoiceItems.Add(New GstInvoiceItemInfo() With {.Description = "Cotton", .BasicRate = 1000, .Hsn_Sc = "1000982",
                                 .Qty = "10", .Uqc = "Kg", .TXVAL = 10000, .RT = 5,
                                 .IAMT = 500, .TY = "G"})


        Dim server As String = "http://www.gstnirvana.com"
        client.PrepareQueryString(server + "/api/data/invoice",
             New Dictionary(Of String, String)())
        Dim _info = client.Post(Of GstInvoiceInfo, ResultInfo(Of Integer))(invoice)

    End Sub

    'Dim provider As New clsClientCredTokenProvider("3U#R62K9J7SR", "KXAFHS", "http://localhost:11626", True)
    'Dim server As String = "http://test.dev:56492"

End Module

