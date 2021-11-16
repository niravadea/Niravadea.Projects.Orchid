using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Niravadea.Projects.Orchid.TokenTracker.Requests;

namespace Niravadea.Projects.Orchid.TokenTracker.Handlers
{
    public class TokenGenerationRequestHandler : IRequestHandler<TokenGenerationRequest, string>
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;

        public TokenGenerationRequestHandler(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public Task<string> Handle(TokenGenerationRequest request, CancellationToken cancellationToken)
        {
            byte[] array = new byte[128 / 8];   // idk, however many bytes 128-bits is.  I really don't feel like opening up calculator, grabbing my phone, or making my neurons do stuff.
            _randomNumberGenerator.GetBytes(array);
            string res = Convert.ToBase64String(array);
            return Task.FromResult(res);
        }
    }
}
