namespace RBC.Models
{
    public class ByteReportReqBody
    {
        public byte[] dataTable { get; set; }
        public string testcode { get; set; }

        public string labcode { get; set; }

        public string sampledate { get; set; }

        public string report_group_id { get; set; }

        public int slno { get; set; }

        public int cnt { get; set; }

        public int flag { get; set; }

        public int endOfReport { get; set; }

        public int currPage { get; set; }

        public int totalPages { get; set; }

        public bool displayReport { get; set; }

        public bool isPE { get; set; }

        public string rptFilePath { get; set; }

        public string reportName { get; set; }

        public string strname1 { get; set; }

        public string tempfilepath { get; set; }

        public string sct { get; set; }

        public string bvt { get; set; }

        public string rrt { get; set; }

        public string customerId { get; set; }

        public int ReportServerIdentifider { get; set; }

        public string dataSet { get; set; }
        public int pageInitialCount { get; set; }
        public int absReportType { get; set; }
    }

    public class ByteReportResponse
    {
        public string CustomerId { get; set; }

        public string Barcodes { get; set; }

        public int isReportAvailable { get; set; }

        public string Status { get; set; }

        public string Filename { get; set; }

        public string reportPath { get; set; }

        public byte[] ByteStream { get; set; }

        public string ByteStreamBase64 { get; set; }

        public string Exception { get; set; }

        public int ByteLength { get; set; }

        public int PageNumber { get; set; }
    }

    public class AllData
    {
        public string SDATE { get; set; }

        public string SL_NO { get; set; }

        public string R3 { get; set; }

        public string PATIENT { get; set; }

        public string REF_DR { get; set; }

        public string LAB_NAME { get; set; }

        public string CUSTOMER_ID { get; set; }

        public string TEST_GROUP { get; set; }

        public string TEST_VALUE { get; set; }

        public string TEST_CODE { get; set; }

        public string DESCRIPTION { get; set; }

        public string FOOTER { get; set; }

        public string SU_CODE1 { get; set; }

        public string METHOD { get; set; }

        public string UNITS { get; set; }
        public string GROUP_ID { get; set; }
        public string PROFILE_CODE { get; set; }

        public string LAB_CODE { get; set; }
        public string SL_NO2 { get; set; }
        public string PREFIX { get; set; }
        public string SU_CODE2 { get; set; }
        public string R_PREFIX { get; set; }
        public string DUMMY_1 { get; set; }
        public string NORMAL_VAL { get; set; }
        public string COMMENTS { get; set; }

        public string DECI { get; set; }
        public string TESTS { get; set; }
        public string BARCODE { get; set; }
        public string SCT { get; set; }
        public string BVT { get; set; }
        public string RRT { get; set; }
        public string SAMPLE_TYPE { get; set; }
        public string METHODOLOGY { get; set; }
        public string REPORT_TEMPLATE { get; set; }
        public string REPORT_GROUP_ID { get; set; }
        public string REPORT_PRINT_ORDER { get; set; }
        public string REMARKS { get; set; }
        public string RATE_TYPE { get; set; }
        public string CUSTOMERID { get; set; }
        public string WOREFNO { get; set; }
        public string ACT_AMOUNT { get; set; }

        public string RELEASEDAT { get; set; }
        public string TSP_ALERT { get; set; }
        public string TEST_REMARK { get; set; }
        public string PDFDTL { get; set; }
        public string REPORTFORMNO { get; set; }
        public string SAMPLETYPE { get; set; }
        public string PATIENTAGE { get; set; }
        public string PATIENTGENDER { get; set; }
        public string RRTSDATE { get; set; }
        public string GROUP_NAME { get; set; }
        public string INDICATOR { get; set; }
        public string INDICATOR2 { get; set; }
        public string ORDER_NO { get; set; }
        public string LOCATION { get; set; }
        public string SIGN1 { get; set; }
        public string SIGN2 { get; set; }

        public string PROCESSED_LOCATION { get; set; }
        public string SOURCEOFWATER { get; set; }
        public string CLINICSIGN { get; set; }
        public string ADDRESS_HTML { get; set; }
        public string LAB_LOCATIONS { get; set; }
        public string WO_MODE { get; set; }
        public string AGE_TYPE { get; set; }
        public string VALUE2 { get; set; }
        public string UPLOAD1 { get; set; }
        public string UPLOAD2 { get; set; }
        public string UPLOAD3 { get; set; }
        public string VALUE1 { get; set; }
        public string VALUE3 { get; set; }
        public string VALUE4 { get; set; }
        public string VALUE5 { get; set; }
        public string VALUE6 { get; set; }

        public string VALUE7 { get; set; }
        public string VALUE8 { get; set; }
        public string VALUE9 { get; set; }
        public string VALUE10 { get; set; }
        public string UPLOAD4 { get; set; }
        public string UPLOAD5 { get; set; }
        public string UPLOAD6 { get; set; }
        public string UPLOAD7 { get; set; }
        public string UPLOAD8 { get; set; }
        public string STATUS { get; set; }
        public string GROUP_TYPE { get; set; }
        public string ADEQUACY_SPECIMEN { get; set; }
        public string GENERAL_CATEGORIZATION { get; set; }
        public string ENDOCERVICAL_CELLS { get; set; }
        public string INFECTION { get; set; }

        public string ATROPHY { get; set; }
        public string REPAIR { get; set; }
        public string ENDOMETRIAL_CELLS { get; set; }
        public string EPITHELIAL_ABNORMALITIES { get; set; }
        public string COMMENT { get; set; }
        public string CYTODIAGNOSIS { get; set; }
        public string ADVISED { get; set; }
        public string CLINICAL_HISTORY { get; set; }
        public string CAP { get; set; }
        public string NABL { get; set; }
        public string contact_no { get; set; }
        public string dob { get; set; }
        public string PanId { get; set; }
        public string COMMENTVALUE { get; set; }
        public string LetterHeadPath { get; set; }

        public string FooterPath { get; set; }
        public string ShowTTLHeader { get; set; }
        public string ShowTTLLogo { get; set; }
        public string Pathologist1Sig { get; set; }
        public string Pathologist1SigPath { get; set; }
        public string Pathologist2Sig { get; set; }
        public string Pathologist2SigPath { get; set; }
        public string NablPath { get; set; }
        public string isReportDynamic { get; set; }
        public string AnalyzerName { get; set; }
        public string SRFID { get; set; }

        public string PE { get; set; }

        public string NABLCertificateNumberpublic { get; set; }
        public string isWhiteLabelled { get; set; }

        public string COLLECTION_TYPE { get; set; }
        public string Amount_Collected { get; set; }
    }
}
