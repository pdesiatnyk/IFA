namespace IfaUdi.Parser.Models
{
    public enum UdiScheme
    {
        Ppn,
        Hpc,
        MasterUdiDi,

        /// <summary>Italy AIC code (PRA-Code "15"). Inner format is not documented by IFA; validated as an opaque code.</summary>
        Aic,

        /// <summary>Portugal AIM (PRA-Code "17"). Inner format is not documented by IFA; validated as an opaque code.</summary>
        Aim,
    }
}
