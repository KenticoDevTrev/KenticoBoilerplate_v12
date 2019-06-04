using System.Collections.Generic;
using CMS.Base;
using CMS.Localization;
using Models.Examples;

namespace Boilerplate.Services.Interfaces
{
    public interface IExampleService
    {
        IEnumerable<ExampleBannersBanner> GetBannersFromNode(ITreeNode node);
        ITreeNode GetCurrentNode();
        IEnumerable<SubNav> GetSubNavFromAliasPath(string nodeAliasPath, CultureInfo cultureInfo, ISiteInfo siteInfo);
        IEnumerable<SubNav> GetSubNavFromNode(ITreeNode node);
    }
}