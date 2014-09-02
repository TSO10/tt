using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public class AgreementsRepository
    {
        public static Agreement GetLastByTime(DateTime time)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from agreement in entities.VEDK_AgreementTbl
                        where agreement.Dato_prissikring > time
                        orderby agreement.Dato_prissikring descending
                        select new Agreement
                        {
                            Agreement_ID = agreement.Agreement_ID,
                            Produktnavn = agreement.Produktnavn,
                            Description = agreement.Description,
                            Pris = agreement.Pris,
                            Dato_prissikring = agreement.Dato_prissikring
                        }).FirstOrDefault();
            }
        }
    }
}
