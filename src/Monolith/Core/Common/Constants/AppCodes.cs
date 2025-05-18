namespace Common.Constants;

/// <summary>
/// Constants for application response messages with corresponding HTTP status codes.
/// </summary>
public static class AppCodes
{
    // ✅ General success and error messages
    public const string SUCCESS = "The operation was completed successfully.";
    public const int SUCCESS_CODE = 200; // OK

    public const string FAILURE = "The operation could not be completed. Please try again.";
    public const int FAILURE_CODE = 400; // Bad Request

    public const string ERROR = "An unexpected error occurred on the server.";
    public const int ERROR_CODE = 500; // Internal Server Error

    public const string INTERNAL_SERVER_ERROR =
        "A server-side error occurred. Please try again later.";
    public const int INTERNAL_SERVER_ERROR_CODE = 500; // Internal Server Error

    // ✅ Authentication & Authorization
    public const string UNAUTHORIZED = "You are not authorized to perform this action.";
    public const int UNAUTHORIZED_CODE = 401; // Unauthorized

    public const string FORBIDDEN = "Access to this resource is denied.";
    public const int FORBIDDEN_CODE = 403; // Forbidden

    public const string ACCESS_DENIED = "You do not have permission to access this resource.";
    public const int ACCESS_DENIED_CODE = 403; // Forbidden

    public const string TOKEN_EXPIRED = "Your session has expired. Please log in again.";
    public const int TOKEN_EXPIRED_CODE = 401; // Unauthorized

    public const string INVALID_TOKEN = "The provided token is invalid or has expired.";
    public const int INVALID_TOKEN_CODE = 401; // Unauthorized

    public const string LOGIN_SUCCESS = "Login was successful.";
    public const int LOGIN_SUCCESS_CODE = 200; // OK

    public const string LOGIN_FAILED = "Login failed. Please check your username and password.";
    public const int LOGIN_FAILED_CODE = 401; // Unauthorized

    public const string LOGOUT_SUCCESS = "You have been logged out successfully.";
    public const int LOGOUT_SUCCESS_CODE = 200; // OK

    public const string SESSION_EXPIRED = "Your session has expired. Please log in again.";
    public const int SESSION_EXPIRED_CODE = 401; // Unauthorized

    // ✅ Request validation and input errors
    public const string BAD_REQUEST = "The request was invalid or cannot be processed.";
    public const int BAD_REQUEST_CODE = 400; // Bad Request

    public const string VALIDATION_FAILED =
        "Validation failed. Please review your input and try again.";
    public const int VALIDATION_FAILED_CODE = 422; // Unprocessable Entity

    public const string MISSING_REQUIRED_FIELDS = "Some required fields are missing.";
    public const int MISSING_REQUIRED_FIELDS_CODE = 422; // Unprocessable Entity

    public const string INVALID_INPUT = "The input data is invalid.";
    public const int INVALID_INPUT_CODE = 422; // Unprocessable Entity

    public const string INVALID_PARAMETERS = "One or more request parameters are invalid.";
    public const int INVALID_PARAMETERS_CODE = 400; // Bad Request

    // ✅ CRUD operations
    public const string CREATED = "The resource was successfully created.";
    public const int CREATED_CODE = 201; // Created

    public const string UPDATED = "The resource was successfully updated.";
    public const int UPDATED_CODE = 200; // OK

    public const string DELETED = "The resource was successfully deleted.";
    public const int DELETED_CODE = 204; // No Content

    public const string CREATE_FAILED = "Failed to create the resource.";
    public const int CREATE_FAILED_CODE = 400; // Bad Request

    public const string UPDATE_FAILED = "Failed to update the resource.";
    public const int UPDATE_FAILED_CODE = 400; // Bad Request

    public const string DELETE_FAILED = "Failed to delete the resource.";
    public const int DELETE_FAILED_CODE = 400; // Bad Request

    // ✅ Resource and data-related
    public const string NOT_FOUND = "The requested resource could not be found.";
    public const int NOT_FOUND_CODE = 404; // Not Found

    public const string ALREADY_EXISTS = "A resource with the same data already exists.";
    public const int ALREADY_EXISTS_CODE = 409; // Conflict

    public const string DUPLICATE_ENTRY = "Duplicate entry detected.";
    public const int DUPLICATE_ENTRY_CODE = 409; // Conflict

    public const string CONFLICT = "A conflict occurred. The operation could not be completed.";
    public const int CONFLICT_CODE = 409; // Conflict

    public const string RESOURCE_LOCKED = "The resource is currently locked or being used.";
    public const int RESOURCE_LOCKED_CODE = 423; // Locked

    public const string RESOURCE_BUSY = "The system is busy. Please try again shortly.";
    public const int RESOURCE_BUSY_CODE = 503; // Service Unavailable

    public const string EMPTY_RESULT = "No data was found for the given criteria.";
    public const int EMPTY_RESULT_CODE = 204; // No Content

    // ✅ File and upload handling
    public const string FILE_UPLOAD_SUCCESS = "The file was uploaded successfully.";
    public const int FILE_UPLOAD_SUCCESS_CODE = 201; // Created

    public const string FILE_UPLOAD_FAILED = "The file upload failed.";
    public const int FILE_UPLOAD_FAILED_CODE = 400; // Bad Request

    public const string FILE_TOO_LARGE = "The uploaded file exceeds the allowed size limit.";
    public const int FILE_TOO_LARGE_CODE = 413; // Payload Too Large

    public const string INVALID_FILE_FORMAT = "The uploaded file format is not supported.";
    public const int INVALID_FILE_FORMAT_CODE = 415; // Unsupported Media Type

    // ✅ Rate limiting, throttling, quota
    public const string TOO_MANY_REQUESTS = "Too many requests. Please slow down.";
    public const int TOO_MANY_REQUESTS_CODE = 429; // Too Many Requests

    public const string RATE_LIMIT_EXCEEDED = "Rate limit exceeded. Try again later.";
    public const int RATE_LIMIT_EXCEEDED_CODE = 429; // Too Many Requests

    public const string QUOTA_EXCEEDED = "You have exceeded your usage quota.";
    public const int QUOTA_EXCEEDED_CODE = 429; // Too Many Requests

    // ✅ Email and notification
    public const string EMAIL_SENT = "The email was sent successfully.";
    public const int EMAIL_SENT_CODE = 200; // OK

    public const string EMAIL_FAILED = "Failed to send the email.";
    public const int EMAIL_FAILED_CODE = 500; // Internal Server Error

    public const string VERIFICATION_EMAIL_SENT =
        "A verification email has been sent to your address.";
    public const int VERIFICATION_EMAIL_SENT_CODE = 200; // OK

    // ✅ Payment and transaction
    public const string PAYMENT_SUCCESS = "The payment was processed successfully.";
    public const int PAYMENT_SUCCESS_CODE = 200; // OK

    public const string PAYMENT_FAILED = "The payment failed. Please try again.";
    public const int PAYMENT_FAILED_CODE = 402; // Payment Required

    public const string PAYMENT_PENDING = "The payment is currently being processed.";
    public const int PAYMENT_PENDING_CODE = 202; // Accepted

    public const string INSUFFICIENT_FUNDS = "Insufficient funds to complete the transaction.";
    public const int INSUFFICIENT_FUNDS_CODE = 402; // Payment Required

    // ✅ System state and maintenance
    public const string SYSTEM_UNDER_MAINTENANCE =
        "The system is under maintenance. Please try again later.";
    public const int SYSTEM_UNDER_MAINTENANCE_CODE = 503; // Service Unavailable

    public const string FEATURE_DISABLED = "This feature is currently disabled.";
    public const int FEATURE_DISABLED_CODE = 503; // Service Unavailable

    // ✅ User management
    public const string USER_CREATED = "User account was successfully created.";
    public const int USER_CREATED_CODE = 201; // Created

    public const string USER_UPDATED = "User profile was successfully updated.";
    public const int USER_UPDATED_CODE = 200; // OK

    public const string USER_DELETED = "User account was successfully deleted.";
    public const int USER_DELETED_CODE = 204; // No Content

    public const string USER_DEACTIVATED = "User account was successfully deactivated.";
    public const int USER_DEACTIVATED_CODE = 200; // OK

    public const string USER_REACTIVATED = "User account was successfully reactivated.";
    public const int USER_REACTIVATED_CODE = 200; // OK

    public const string USER_NOT_FOUND = "The requested user could not be found.";
    public const int USER_NOT_FOUND_CODE = 404; // Not Found

    public const string USER_ALREADY_EXISTS = "A user with this username or email already exists.";
    public const int USER_ALREADY_EXISTS_CODE = 409; // Conflict

    public const string USERNAME_TAKEN =
        "This username is already taken. Please choose another one.";
    public const int USERNAME_TAKEN_CODE = 409; // Conflict

    public const string EMAIL_ALREADY_REGISTERED = "This email address is already registered.";
    public const int EMAIL_ALREADY_REGISTERED_CODE = 409; // Conflict

    // ✅ User authentication & password management
    public const string PASSWORD_RESET_REQUESTED =
        "Password reset instructions have been sent to your email.";
    public const int PASSWORD_RESET_REQUESTED_CODE = 200; // OK

    public const string PASSWORD_RESET_SUCCESS = "Your password has been reset successfully.";
    public const int PASSWORD_RESET_SUCCESS_CODE = 200; // OK

    public const string PASSWORD_RESET_FAILED = "Failed to reset your password. Please try again.";
    public const int PASSWORD_RESET_FAILED_CODE = 400; // Bad Request

    public const string PASSWORD_CHANGED = "Your password has been changed successfully.";
    public const int PASSWORD_CHANGED_CODE = 200; // OK

    public const string PASSWORD_MISMATCH =
        "The new password and confirmation password do not match.";
    public const int PASSWORD_MISMATCH_CODE = 422; // Unprocessable Entity

    public const string PASSWORD_TOO_WEAK = "Password does not meet the strength requirements.";
    public const int PASSWORD_TOO_WEAK_CODE = 422; // Unprocessable Entity

    public const string PASSWORD_RECENTLY_USED =
        "This password was recently used. Please choose a different one.";
    public const int PASSWORD_RECENTLY_USED_CODE = 422; // Unprocessable Entity

    public const string ACCOUNT_LOCKED =
        "Your account has been locked due to multiple failed login attempts.";
    public const int ACCOUNT_LOCKED_CODE = 423; // Locked

    public const string ACCOUNT_UNLOCKED = "Your account has been unlocked successfully.";
    public const int ACCOUNT_UNLOCKED_CODE = 200; // OK

    // ✅ User verification & 2FA
    public const string EMAIL_VERIFICATION_REQUIRED =
        "Please verify your email address to continue.";
    public const int EMAIL_VERIFICATION_REQUIRED_CODE = 403; // Forbidden

    public const string EMAIL_VERIFIED = "Your email address has been verified successfully.";
    public const int EMAIL_VERIFIED_CODE = 200; // OK

    public const string PHONE_VERIFICATION_REQUIRED =
        "Please verify your phone number to continue.";
    public const int PHONE_VERIFICATION_REQUIRED_CODE = 403; // Forbidden

    public const string PHONE_VERIFIED = "Your phone number has been verified successfully.";
    public const int PHONE_VERIFIED_CODE = 200; // OK

    public const string MFA_ENABLED =
        "Multi-factor authentication has been enabled for your account.";
    public const int MFA_ENABLED_CODE = 200; // OK

    public const string MFA_DISABLED =
        "Multi-factor authentication has been disabled for your account.";
    public const int MFA_DISABLED_CODE = 200; // OK

    public const string MFA_REQUIRED = "Multi-factor authentication is required for this action.";
    public const int MFA_REQUIRED_CODE = 403; // Forbidden

    public const string MFA_CODE_SENT = "A verification code has been sent to your device.";
    public const int MFA_CODE_SENT_CODE = 200; // OK

    public const string MFA_CODE_INVALID = "The verification code is invalid or has expired.";
    public const int MFA_CODE_INVALID_CODE = 422; // Unprocessable Entity

    // ✅ User roles & permissions
    public const string ROLE_ASSIGNED = "The role has been successfully assigned to the user.";
    public const int ROLE_ASSIGNED_CODE = 200; // OK

    public const string ROLE_REMOVED = "The role has been successfully removed from the user.";
    public const int ROLE_REMOVED_CODE = 200; // OK

    public const string PERMISSION_GRANTED =
        "The permission has been successfully granted to the user.";
    public const int PERMISSION_GRANTED_CODE = 200; // OK

    public const string PERMISSION_REVOKED =
        "The permission has been successfully revoked from the user.";
    public const int PERMISSION_REVOKED_CODE = 200; // OK

    public const string INSUFFICIENT_PERMISSIONS =
        "You have insufficient permissions to perform this action.";
    public const int INSUFFICIENT_PERMISSIONS_CODE = 403; // Forbidden

    // ✅ User profile & preferences
    public const string PROFILE_UPDATED = "Your profile has been updated successfully.";
    public const int PROFILE_UPDATED_CODE = 200; // OK

    public const string AVATAR_UPDATED = "Your profile picture has been updated successfully.";
    public const int AVATAR_UPDATED_CODE = 200; // OK

    public const string PREFERENCES_UPDATED = "Your preferences have been updated successfully.";
    public const int PREFERENCES_UPDATED_CODE = 200; // OK

    public const string NOTIFICATION_SETTINGS_UPDATED =
        "Your notification settings have been updated successfully.";
    public const int NOTIFICATION_SETTINGS_UPDATED_CODE = 200; // OK

    public const string PRIVACY_SETTINGS_UPDATED =
        "Your privacy settings have been updated successfully.";
    public const int PRIVACY_SETTINGS_UPDATED_CODE = 200; // OK

    // ✅ User session management
    public const string SESSION_CREATED = "A new session has been created successfully.";
    public const int SESSION_CREATED_CODE = 201; // Created

    public const string SESSION_TERMINATED = "The session has been terminated successfully.";
    public const int SESSION_TERMINATED_CODE = 200; // OK

    public const string ALL_SESSIONS_TERMINATED =
        "All your sessions have been terminated successfully.";
    public const int ALL_SESSIONS_TERMINATED_CODE = 200; // OK

    public const string ACTIVE_SESSIONS_LISTED = "Active sessions retrieved successfully.";
    public const int ACTIVE_SESSIONS_LISTED_CODE = 200; // OK

    public const string SESSION_NOT_FOUND = "The specified session could not be found.";
    public const int SESSION_NOT_FOUND_CODE = 404; // Not Found

    // ✅ Social & Third-party authentication
    public const string SOCIAL_AUTH_SUCCESS = "Social authentication was successful.";
    public const int SOCIAL_AUTH_SUCCESS_CODE = 200; // OK

    public const string SOCIAL_AUTH_FAILED = "Social authentication failed. Please try again.";
    public const int SOCIAL_AUTH_FAILED_CODE = 401; // Unauthorized

    public const string ACCOUNT_LINKED = "Your account has been linked successfully.";
    public const int ACCOUNT_LINKED_CODE = 200; // OK

    public const string ACCOUNT_UNLINKED = "Your account has been unlinked successfully.";
    public const int ACCOUNT_UNLINKED_CODE = 200; // OK

    public const string SOCIAL_ACCOUNT_ALREADY_LINKED =
        "This social account is already linked to another user.";
    public const int SOCIAL_ACCOUNT_ALREADY_LINKED_CODE = 409; // Conflict
}
