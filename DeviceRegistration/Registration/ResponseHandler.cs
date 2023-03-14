using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceRegistration.Registration
{
    /// <summary>
    /// Handles the API call reponse/s.
    /// </summary>
    public class ResponseHandler : IResponseHandler
    {
        //output - prefix-productcode-IMEI/SerialNumber
        //Following Serial Numbers Registered: EVD-MKH-FAKETEST1-uniqueIMEI123456789, EVD-MKH-FAKETEST1-unique2IMEI123456789 | Following Serial Numbers can not be registered with given product code: EVD-FAKETEST1-unique2IMEI123456789
        /// <summary>
        /// Processes a registration response and produces a <see cref="RegistrationResult"/>.
        /// </summary>
        /// <param name="response">A registration response string.</param>
        /// <returns>A <see cref="RegistrationResult"/>.</returns>
        public RegistrationResult GetRegistrationResult(string response)
        {
            RegistrationResult registrationResult = new RegistrationResult();
            if (response is not null)
            {
                String[] separator = { ": ", "," };
                String[] initialString = response.Split("|");
                if (initialString is not null)
                {
                    if (initialString.Length > 0)
                    {
                        String[] successItems = initialString[0].Split(separator, StringSplitOptions.TrimEntries);
                        registrationResult.RegistrationSuccess = ProcessResponse(successItems, true);
                    }
                    if (initialString.Length == 2)
                    {
                        String[] failedItems = initialString[1].Split(separator, StringSplitOptions.TrimEntries);
                        registrationResult.RegistrationFailure = ProcessResponse(failedItems, false);
                    }
                }
            }
            return registrationResult;
        }

        /// <summary>
        /// Processes a success or failed reponse.
        /// </summary>
        /// <param name="response">A success or failed response string array.</param>
        /// <param name="isSuccess">Indicates success or failure.</param>
        /// <returns>Returns a <see cref="RegistrationResponse"/>.</returns>
        public RegistrationResponse ProcessResponse(string[] response, bool isSuccess)
        {
            RegistrationResponse registrationResponse = new RegistrationResponse();
            registrationResponse.Items = new List<IRegistrationItem>();
            registrationResponse.Message = response[0];
            registrationResponse.Succeeded = isSuccess;
            if(isSuccess)
            {
                for (int i = 1; i < response.Length; i++)
                {
                    SuccessfulRegistrationItem item = new SuccessfulRegistrationItem()
                    {
                        RegistrationResponseString = response[i]
                    };
                    registrationResponse.Items.Add(item);
                }
            }
            else
            {
                for (int i = 1; i < response.Length; i++)
                {
                    IRegistrationItem item = new FailedRegistrationItem()
                    {
                        RegistrationResponseString = response[i]
                    };
                    registrationResponse.Items.Add(item);
                }
            }

            return registrationResponse;
        }

    }
}
