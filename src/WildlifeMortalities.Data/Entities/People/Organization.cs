namespace WildlifeMortalities.Data.Entities.People;

public class Organization : PersonWithAuthorizations
{
    public override void Update(PersonWithAuthorizations person)
    {
        if (person is Organization organization)
        {
            Update(organization);
        }
    }

    public void Update(Organization organization)
    {
        LastModifiedDateTime = organization.LastModifiedDateTime;
    }

    public override bool Merge(PersonWithAuthorizations person)
    {
        if (person is Organization organization)
        {
            return Merge(organization);
        }
        return false;
    }

    public bool Merge(Organization organizationToBeMerged)
    {
        throw new NotImplementedException();
    }
}
