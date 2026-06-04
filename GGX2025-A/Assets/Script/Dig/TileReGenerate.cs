using System.Collections.Generic;
using UnityEngine;

public class TileReGenerate : MonoBehaviour
{
    [SerializeField] GameObject tilemask_point;
    [SerializeField] GameObject tilemask_rect;
    [SerializeField] GameObject tilemask_L;
    [SerializeField] GameObject tilemask_square;
    [SerializeField] GameObject tilemask_check;
    [SerializeField] GameObject currentmask;

    public bool tilecol_UL = true;//����
    public bool tilecol_UR = true;//�E��
    public bool tilecol_LL = true;//����
    public bool tilecol_LR = true;//�E��

    [SerializeField] int pattern;

    public AudioClip digSE;

    public static Queue<float> recentSoundTimes = new Queue<float>();
    private const int maxSimultaneousSounds = 7;  // �����ɖ��Ă����ő吔
    private const float soundTimeWindow = 0.1f;   // ���b�ȓ��̍Đ����u�����v�Ƃ݂Ȃ���


    public void regenerate()
    {
        if (currentmask != null)
            Destroy(currentmask);

        // �v���n�u�Ɖ�]�p�x���擾
        (GameObject prefab, float rotZ) = SelectPrefabAndRotation();

        // �w�肵����]�p�Ő���
        currentmask = Instantiate(
            prefab,
            transform.position,
            Quaternion.Euler(0, 0, rotZ),
            transform
        );
    }
    private (GameObject prefab, float rotZ) SelectPrefabAndRotation()
    {
        pattern = 0;
        if (tilecol_UL) pattern += 1;
        if (tilecol_UR) pattern += 2;
        if (tilecol_LL) pattern += 4;
        if (tilecol_LR) pattern += 8;

        if (pattern == 0)
        {
            while (recentSoundTimes.Count > 0 && Time.time - recentSoundTimes.Peek() > soundTimeWindow)
            {
                recentSoundTimes.Dequeue();
            }

            // 2. ���݂��̎��ԓ��ɖ����������(maxSimultaneousSounds)�����Ȃ�炷
            if (recentSoundTimes.Count < maxSimultaneousSounds)
            {
                SEManager.Instance.Play(digSE);
                recentSoundTimes.Enqueue(Time.time); // �炵�����Ԃ��L�^
            }

            ParticleController.Instance.PlayDestroyEffect(transform.position);
            Destroy(gameObject);
        }


        switch (pattern)
        {
            // ����
            case 1:
                return (tilemask_point, -90f);

            // �E��
            case 2:
                return (tilemask_point, -180f);

            //����
            case 3: 
                return (tilemask_rect, -180f);

            //����
            case 4:
                return (tilemask_point, 0f);

            //�����
            case 5:
                return (tilemask_rect, -90f);

            //�����ƉE��
            case 6:
                return (tilemask_check, 0f);

            //�E���ȊO
            case 7:
                return (tilemask_L, -90f);

            //�E��
            case 8:
                return (tilemask_point, 90f);

            //����ƉE��
            case 9:
                return (tilemask_check, 90f);

            //�E���
            case 10:
                return (tilemask_rect, 90f);

            //�����ȊO
            case 11:
                return (tilemask_L, 180f);

            //�����
            case 12:
                return (tilemask_rect, 0f);

            //�E��ȊO
            case 13:
                return (tilemask_L, 0f);

            //����ȊO
            case 14:
                return (tilemask_L, 90f);

            //�S������
            default:
                return (tilemask_square, 0f);
        }
    }
}
