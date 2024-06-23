namespace cimbar.lib
{
    public class polyomial
    {

        polynomial_t polynomial_create(int order)
        {
            polynomial_t polynomial = new polynomial_t();
            polynomial.coeff = new byte[order + 1];
            polynomial.order = order;
            return polynomial;
        }


        static byte field_div_log(field_t field, byte l, byte r)
        {
            // like field_mul_log, this performs field_div without going through a field_element_t
            ushort res = (ushort)((ushort)255 + (ushort)l - (ushort)r);
            if (res > 255)
            {
                return (byte)(res - 255);
            }
            return (byte)res;
        }

        static byte field_add(field_t field, byte l, byte r)
        {
            return (byte)(l ^ r);
        }
        static byte field_mul_log_element(field_t field, byte l, byte r)
        {
            // like field_mul_log, but returns a field_element_t
            // because we are doing lookup here, we can safely skip the wrapover check
            ushort res = (ushort)((ushort)l + (ushort)r);
            return field.exp[res];
        }

        public static void polynomial_mod(field_t field, polynomial_t dividend, polynomial_t divisor, polynomial_t mod)
        {
            // find the polynomial remainder of dividend mod divisor
            // do long division and return just the remainder (written to mod)

            if (mod.order < dividend.order)
            {
                // mod.order must be >= dividend.order (scratch space needed)
                // this is an error -- catch it in debug?
                return;
            }
            // initialize remainder as dividend
            Array.Copy(mod.coeff, dividend.coeff, sizeof(byte) * (dividend.order + 1));
            //memcpy(mod.coeff, dividend.coeff, sizeof(byte) * (dividend.order + 1));

            // XXX make sure divisor[divisor_order] is nonzero
            byte divisor_leading = field.log[divisor.coeff[divisor.order]];
            // long division steps along one order at a time, starting at the highest order
            for (int i = dividend.order; i > 0; i--)
            {
                // look at the leading coefficient of dividend and divisor
                // if leading coefficient of dividend / leading coefficient of divisor is q
                //   then the next row of subtraction will be q * divisor
                // if order of q < 0 then what we have is the remainder and we are done
                if (i < divisor.order)
                {
                    break;
                }
                if (mod.coeff[i] == 0)
                {
                    continue;
                }
                int q_order = i - divisor.order;
                byte q_coeff = field_div_log(field, field.log[mod.coeff[i]], divisor_leading);

                // now that we've chosen q, multiply the divisor by q and subtract from
                //   our remainder. subtracting in GF(2^8) is XOR, just like addition
                for (int j = 0; j <= divisor.order; j++)
                {
                    if (divisor.coeff[j] == 0)
                    {
                        continue;
                    }
                    // all of the multiplication is shifted up by q_order places
                    mod.coeff[j + q_order] = field_add(field, mod.coeff[j + q_order],
                                field_mul_log_element(field, field.log[divisor.coeff[j]], q_coeff));
                }
            }
        }

    }
}