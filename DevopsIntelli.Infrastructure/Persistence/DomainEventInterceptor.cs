using DevopsIntelli.Domain.Common.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace DevopsIntelli.Infrastructure.Persistence
{
    public  class DomainEventInterceptor:SaveChangesInterceptor
    {
        private readonly IPublisher _publisher;

        public DomainEventInterceptor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        
      
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
       DbContextEventData eventData,
       InterceptionResult<int> result,
       CancellationToken cancellationToken = default)
        {
        
            await PublishDomainEvents(eventData?.Context!, cancellationToken);
            return result;
        }
        private async Task PublishDomainEvents(DbContext context, CancellationToken ct)
        {
            if (context == null)
                return;
            var entities = context.ChangeTracker.Entries<BaseEntity>()
                //where theres any domainevent
                .Where(e => e.Entity.DomainEvents.Any())
                //seelect entity then
                .Select(e => e.Entity).ToList();

            var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
            //clear domain events after that
            entities.ForEach(e => e.ClearDomainEvent());
            
            foreach(var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent,ct);
            }
        }
    }
}
