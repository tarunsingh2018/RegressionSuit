using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Utilities.Common
{
    class Assertions
    {
        public const string TABLE_NOT_FOUND = "Table not found for xpath: ({0})";

        // Table row assertions
        public const string TR_GT = "Table rows for xpath: ({0}) > {1}";
        public const string TR_NOT_GT = "! Table rows for xpath: ({0}) > {1}";
        public const string TR_LT = "Table rows for xpath: ({0}) < {1}";
        public const string TR_NOT_LT = "! Table rows for xpath: ({0}) < {1}";
        public const string TR_EQ = "Table rows for xpath: ({0}) == {1}";
        public const string TR_NOT_EQ = "Table rows for xpath: ({0}) != {1}";
        public const string TR_GT_EQ = "Table rows for xpath: ({0}) >= {1}";
        public const string TR_NOT_GT_EQ = "! Table rows for xpath: ({0}) >= {1}";
        public const string TR_LT_EQ = "Table rows for xpath: ({0}) <= {1}";
        public const string TR_NOT_LT_EQ = "! Table rows for xpath: ({0}) <= {1}";

        // Table data assertions
        public const string TD_GT = "Table data for xpath: ({0}) > {1}";
        public const string TD_NOT_GT = "! Table data for xpath: ({0}) > {1}";
        public const string TD_IN_TR_NOT_GT = "! Table data in table row xpath: ({0}) > {1}";
        public const string TD_LT = "Table data for xpath: ({0}) < {1}";
        public const string TD_NOT_LT = "! Table data for xpath: ({0}) < {1}";
        public const string TD_EQ = "Table data for xpath: ({0}) == {1}";
        public const string TD_NOT_EQ = "Table data for xpath: ({0}) != {1}";
        public const string TD_GT_EQ = "Table data for xpath: ({0}) >= {1}";
        public const string TD_NOT_GT_EQ = "! Table data for xpath: ({0}) >= {1}";
        public const string TD_LT_EQ = "Table data for xpath: ({0}) <= {1}";
        public const string TD_NOT_LT_EQ = "! Table data for xpath: ({0}) <= {1}";

        // List assertions
        public const string LIST_SIZE = "List size = ";
        public const string LIST_IS_EMPTY = "List for xpath: ({0}) is empty";
        public const string LIST_IS_NOT_EMPTY = "List for xpath: ({0}) is not empty";
        public const string LIST_SIZE_GT = "List size for xpath: ({0}) > {1}";
        public const string LIST_SIZE_NOT_GT = "! List size for xpath: ({0}) > {1}";
        public const string LIST_SIZE_LT = "List size for xpath: ({0}) < {1}";
        public const string LIST_SIZE_NOT_LT = "! List size for xpath: ({0}) < {1}";
        public const string LIST_SIZE_EQ = "List size for xpath: ({0}) == {1}";
        public const string LIST_SIZE_IS_EQ = "List size for xpath: ({0}) = {1}";
        public const string LIST_SIZE_NOT_EQ = "List size for xpath: ({0}) != {1}";
        public const string LIST_SIZE_GT_EQ = "List size for xpath: ({0}) >= {1}";
        public const string LIST_SIZE_NOT_GT_EQ = "! List size for xpath: ({0}) >= {1}";
        public const string LIST_SIZE_LT_EQ = "List size for xpath: ({0}) <= {1}";
        public const string LIST_SIZE_NOT_LT_EQ = "! List size for xpath: ({0}) <= {1}";
        public const string LIST_CONTAINS_ITEM = "List for xpath: ({0}) contains ({1})";
        public const string LIST_NOT_CONTAINS_ITEM = "! List for xpath: ({0}) contains ({1})";
        public const string LIST_W_HEADER_NOT_CONTAINS = "{0} :\n! List for xpath: ({1}) contains ({2})";

        // Data checking assertions
        public const string DATA_VALUE_NULL = "Value for ({0}) == null";
        public const string DATA_VALUE_NOT_NULL = "Value for ({0}) != null";
        public const string DATA_VALUE_MATCHES = "({0}) matches \"{1}\"";
        public const string DATA_VALUE_SHOULD_MATCH = "({0}) is not empty & should match the format - \"{1}\"";
        public const string DATA_VALUE_DOES_NOT_MATCH = "({0}) does not match \"{1}\"";
        public const string DATA_DOES_NOT_MATCH_WITH = "({0}) does not match with any of \"{1}\"";
        public const string DATA_VALUE_NOT_EQ = "({0}) != \"{1}\"";
        public const string DATA_VALUE_EQUALS_TO = "({0}) == \"{1}\"";
        public const string DATA_VALUE_EQ = "({0}) == \"{1}\"";
        public const string DATA_VALUE_GT = "({0}) > \"{1}\"";
        public const string DATA_VALUE_GT_EQ = "({0}) >= \"{1}\"";
        public const string DATA_VALUE_NOT_GT_EQ = "! ({0}) >= \"{1}\"";
        public const string DATA_VALUE_NOT_GT = "! ({0}) > \"{1}\"";
        public const string DATA_VALUE_FOUND = "({0}) is found";
        public const string DATA_VALUE_NOT_FOUND = "({0}) is not found";
        public const string DATA_VALUES_NOT_FOUND = "({0}) are not found";
        public const string DATA_VALUE_INCORRECT_FOR = "({0}) is incorrect for ({1})";
        public const string DATA_VALUE_EMPTY = "({0}) is empty";
        public const string ELEMENT_TEXT_NOT_EQ = "Element text ({0}) != \"{1}\"";
        public const string ELEMENT_TEXT_NOT_EMPTY_FOR = "Element text is not empty for ({0})";
        public const string ELEMENT_TEXT_NOT_EMPTY_WITH = "Element text is not empty for ({0}) with text ({1})";
        public const string ELEMENT_IS_ENABLED = "Element with label/xpath ({1}) is enabled";
        public const string ELEMENT_IS_NOT_ENABLED = "Element with label/xpath ({0}) is not enabled";
        public const string ELEMENT_IS_DISABLED = "Element with label/xpath ({0}) is disabled";
        public const string ELEMENT_IS_NOT_DISABLED = "Element with label/xpath ({0}) is not disabled";
        public const string SELECT_TEXT_NOT_DEFAULT = "Select option text not set to either of defaults ({0}) for ({1}) with text ({2})";
        public const string ELEMENT_TEXT_EMPTY_FOR = "Element text is empty for ({0})";
        public const string ELEMENT_TEXT_EMPTY = "Element text is empty for";
        public const string ELEMENT_WITH_NAME_EMPTY = "Element with \"name={0}\" is empty";
        public const string DATA_VALUE_NOT_EMPTY = "({0}) is not empty";
        public const string DATA_VALUE_DO_NOT_CONTAIN = "({0}) does not contain \"{1}\"";
        public const string DOES_NOT_CONTAIN_EITHER_ALL = "({0}) does not contain either/all of \"{1}\"";
        public const string EITHER_OR_ALL_NOT_FOUND = "Either or all of [{0}] not found";
        public const string EITHER_OR_ALL_OF_LIST_NOT_FOUND = "Either or all of {0} not found";
        public const string EITHER_OR_ALL_FOUND = "Either or all of [{0}] found";
        public const string EITHER_OR_ALL = "Either or all of [{0}]";
        public const string DATA_VALUE_CONTAINS = "({0}) contains \"{1}\"";
        public const string FILE_IS_EMPTY = "File ({0}) is empty";
        public const string FILE_IS_NOT_EMPTY = "File ({0}) is not empty";
        public const string ATTRIBUTE_VALUE_EMPTY = "Attribute: ({0}) of ({1}) is empty";

        // Browser Windows
        public const string OPEN_WINDOWS_EQ = "({0}) == \"{1}\"";
        public const string OPEN_WINDOWS_NOT_EQ = "({0}) != \"{1}\"";
        public const string OPEN_WINDOWS_GT = "({0}) > \"{1}\"";
        public const string OPEN_WINDOWS_NOT_GT = "! ({0}) > \"{1}\"";
        public const string OPEN_WINDOWS_LT = "({0}) < \"{1}\"";
        public const string OPEN_WINDOWS_NOT_LT = "! ({0}) < \"{1}\"";

        // Element
        public const string ELEMENT_NOT_FOUND = "Could not find {0} element with XPath: ({1})";
        public const string ELEMENT_FOUND = "Found {0} element with XPath: ({1})";
        public const string ELEMENT_NOT_DISPLAYED = "Element not displayed with XPath: ({0})";
        public const string ELEMENTS_NOT_DISPLAYED = "Elements not displayed with XPath: ({0})";
        public const string ELEMENTS_NOT_FOUND = "Elements not found for XPath: ({0})";
        public const string SELECT_OPTIONS_NOT_FOUND = "Select options not found for XPath: ({0})";
        public const string MESSAGE_ON_GIVEN_PAGE = "Message - ({0}) on {1} page";

        // Page
        public const string PAGE_NOT_OPENED = "\"{0}\" page has not opened";

        public const string ITEM_SELECTED = "Item - \"{0}\" is selected";
        public const string ITEM_NOT_SELECTED = "Item - \"{0}\" is not selected";

        public const string BEFORE = "Before \"{0}\": {1}";
        public const string AFTER = "After \"{0}\": {1}";
        public const string TO_CONTINUE = "\"{0}\" to continue ...!!!";
        public const string COULD_NOT_FOUND_WITH = "Could not find {0} with {1}";
    }
}
