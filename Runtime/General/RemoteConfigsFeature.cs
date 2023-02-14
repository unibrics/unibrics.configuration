namespace Unibrics.Configuration.General
{
    using Core.Features;

    [AppFeature]
    public class RemoteConfigsFeature : AppFeature
    {
        public override string Id => "remote.configs";

        public override bool SupportsSuspension => true;
        public override bool DefaultSuspensionValue => false;
        public override bool DoNotResetValueWithSave => true;
    }
}