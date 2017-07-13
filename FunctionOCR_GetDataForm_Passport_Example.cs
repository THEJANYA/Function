   
struct data {
        public string CODE;
        public string COUNTRY;
        public string SURNAME;
        public string NAME;
        public string PASSNO;
        public string NATION;
        public DateTime BIRTHDATE;
        public string SEX;
        public DateTime EXPIREDATE;
        public string PERSONALNO;
        public string EXCEPTION;
 };
 //string reg_text = ^(?<DATA>.+)(?<CODE>P<|PA|PV)(?<SEP0>[<]{0,1})(?<COUNTRY>[A-Z]{3})(?<SURNAME>[a-z,A-Z,0-9]{1,45})(?<SEP1>[<]{2})(?<NAME>[a-z,A-Z,0-9,<]{1,45})(?<SEP2>[<]{3})(?<PASSNO>[A-Z,0-9]{7,9})(?<SEP3>[<]{0,3})(?<CHECK1>[0-9]{0,1})(?<NATION>[A-Z,0]{3})(?<BIRTHDATE>[0-9,O]{6})(?<CHECK2>[0-9,<]{0,1})(?<SEX>[MF<]{1})(?<EXPIREDATE>[0-9,O]{6})(?<CHECK3>[0-9,<]{0,1})(?<PERSONALNO>[A-Z,0-9,<]{14})(?<CHECK4>[0-9,<]{1})(?<CHECK5>[0-9,<]{1}).{0,}
 public Processdata GetOCRData(string pic_path, string reg_text)
        {
            #region Nicomsoft OCR
            int CfgObj, OcrObj, ImgObj, res;
            string txt;
            NSOCRLib.NSOCRClass NsOCR = new NSOCRLib.NSOCRClass(); //create NSOCR COM object instance
            NsOCR.Engine_Initialize(); //initialize OCR engine
            NsOCR.Cfg_Create(out CfgObj); //create CFG object
            NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); //load configuration
            NsOCR.Ocr_Create(CfgObj, out OcrObj); //create OCR object
            NsOCR.Img_Create(OcrObj, out ImgObj); //create IMG object
            
            res = NsOCR.Img_LoadFile(ImgObj, pic_path); //load some image for OCR
            if (res > TNSOCR.ERROR_FIRST) { }; //insert error handler here
            res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_LAST, TNSOCR.OCRFLAG_NONE); //perform OCR
            if (res > TNSOCR.ERROR_FIRST) { }; //insert error handler here
            NsOCR.Img_GetImgText(ImgObj, out txt, TNSOCR.FMT_EXACTCOPY); //get text
            //use "txt" variable now
            NsOCR.Engine_Uninitialize(); //release all created objects and uninitialize OCR engine

            txt = txt.Replace("\r\n", "");
            #endregion Nicomsoft OCR

            #region RegularExpression
            
                data Obj = new data();
                try
                {
                    Regex Reg = new Regex(@reg_text);

                    // Find matches.
                    MatchCollection matches = Reg.Matches(txt);

                    // Report on each match.
                    foreach (Match match in matches)
                    {
                        GroupCollection groups = match.Groups;
                        //^(?<DATA>.+)(?<CODE>P<)(?<COUNTRY>[A-Z]{3})(?<SURNAME>[A-Z]{1,39})(?<SEP1>[<]{1,2})(?<NAME>[A-Z]{1,39})(?<SEP2>[<]{1,39})(?<PASSNO>[A-Z,0-9]{7,9})(?<SEP3>[<]{0,2})(?<CHECK1>[0-9]{1})(?<NATION>[A-Z]{3})(?<BIRTHDATE>[0-9]{6})(?<CHECK2>[0-9]{1})(?<SEX>[MF<]{1})(?<EXPIREDATE>[0-9]{6})(?<CHECK3>[0-9]{1})(?<PERSONALNO>[A-Z,0-9,<]{14})(?<CHECK4>[0-9]{1})(?<CHECK5>[0-9]{1}).{0,}
                        Obj.CODE = groups["CODE"].Value;
                        Obj.COUNTRY = groups["COUNTRY"].Value;
                        Obj.SURNAME = groups["SURNAME"].Value.Replace("6", "G");
                        Obj.NAME = groups["NAME"].Value.Replace("6", "G").Replace("<", " ").Trim();
                        Obj.PASSNO = groups["PASSNO"].Value;
                        Obj.NATION = groups["NATION"].Value.Replace("0", "O");
                        Obj.BIRTHDATE = DateTime.ParseExact(groups["BIRTHDATE"].Value.Replace("O", "0"), "yyMMdd", null);
                        Obj.SEX = groups["SEX"].Value;
                        Obj.EXPIREDATE = DateTime.ParseExact(groups["EXPIREDATE"].Value.Replace("O", "0"), "yyMMdd", null);
                        Obj.PERSONALNO = groups["PERSONALNO"].Value;
                        Obj.EXCEPTION = "NONE";
                    }
                    if (string.IsNullOrEmpty(Obj.EXCEPTION))
                    {
                        Obj.EXCEPTION = txt;
                    }
                }
                catch (Exception ex)
                {
                    Obj.EXCEPTION = ex.Message.ToString();
                }
            #endregion RegularExpression

      return Obj;
}
