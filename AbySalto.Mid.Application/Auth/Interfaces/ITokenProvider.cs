using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Auth.Interfaces;

public interface ITokenProvider
{
    string Generate(User user);
}
