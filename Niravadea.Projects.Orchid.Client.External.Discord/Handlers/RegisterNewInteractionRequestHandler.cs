using MediatR;
using Niravadea.Projects.Orchid.Client.External.Discord.Requests;
using Niravadea.Projects.Orchid.Client.External.Discord.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.External.Discord.Handlers
{
    public class RegisterNewInteractionRequestHandler : IRequestHandler<RegisterNewInteractionRequest, ulong>
    {
        private readonly IInteractionManager _interactionManager;

        public RegisterNewInteractionRequestHandler(IInteractionManager interactionManager)
        {
            _interactionManager = interactionManager;
        }

        public async Task<ulong> Handle(
            RegisterNewInteractionRequest request,
            CancellationToken cancellationToken
        ) =>
            await _interactionManager.RegisterNewInteraction(
                context: request.Context
            );
    }
}
