# Intro2SE_20CLC03_Group04

## Thông tin Project

Đồ án của nhóm sẽ lập trình một ứng dụng trò chơi theo dạng đánh cờ vua, các quân cờ được hiện diện trên 1 bàn cờ và được điều khiển bởi người chơi và các quân cờ sẽ có những chức năng riêng của từng quân, nhầm tang them sức lôi cuốn và đồng thời đem đến những cảm giác cho cách chơi và lối suy nghĩ mới lạ cho người chơi.

## Miêu tả trò chơi

Trò chơi sẽ xoay quanh ý tưởng của những trận đánh cờ tuy đơn giản nhưng yêu cần người chơi sẽ phải có chiến lượt của riêng mình để đánh thắng được đối thủ.
Các nhân vật trong trò chơi sẽ là những nhân vật cơ bản trong cơ vua (chốt, ngựa, tượng, xe) và thêm vào đó là những quân bài chủ chốt, có chức năng để thay cho hậu và vua.
Trò chơi sẽ phục vụ cho khán giả vào lứa tuổi vị thành niên và trở lên (13+) và sẽ được thực hiện trên môi trường làm viêc của Unity kết hợp với lập trình bằng ngôn ngữ lập trình C#.

## Các tính năng của trò chơi

### Quân cờ

Các quân cờ cơ bản của cờ vua (chốt, ngựa, tượng, xe) sẽ di chuyển như trong các trận cờ vua cổ điển:

- Quân Chốt: di chuyển theo từng ô, có thể đi 2 ô trong bước đi đầu tiên và ăn quân đứng chéo với mình.

- Quân Ngựa: di chuyển theo hình chữ L và ăn quân cờ ở vị trí đích đến của mình.

- Quân Tượng: di chuyển theo đường chéo và ăn quân cờ ở vị trí đích đến của mình, không thể đi xuyên qua các quân cờ khác.

- Quân Xe: di chuyển theo các đường thẳng và ăn quân cờ ở vị trí đích đến của mình, không thể đi xuyên qua các quân cờ khác.

### Quân anh hùng

Ở trò chơi này sẽ không có quân Hậu hay Vua, nhưng thay vào đó sẽ là quân cờ Anh Hùng (Hero) sẽ có chức năng riêng cho từng quân, tùy chọn bởi người chơi. Cờ anh hùng, có thể đi theo đường chéo, và đi theo đường thẳng, nhưng cờ anh hùng khi bị giết sẽ không chết mà sẽ được hồi sinh trong một số lượt nhất định.
Chướng ngại vật:

Ngoài những quân tướng đã nói trên, trên bàn cờ còn sẽ có những vật cản, địa hình, mô hình buộc người chơi phải tìm cách đi qua các chướng ngại vật ấy.

### Cách chơi

Người chơi sẽ phải lên chiến lượt và điều khiển các quân cờ của mình chiến đấu với đối phương và trò chơi sẽ kết thúc khi thõa điều kiện thắng của bản thân hoặc thua trước tay của đối thủ.

#### Điều kiện thắng

- Trên bàn cờ phải có ít nhất 2 quân cờ còn sống.
- Đối thủ chỉ còn nhiều nhất 1 quân cờ hoặc quân anh hùng trên bàn cờ.

#### Điều kiện thua

- Trên bàn cờ chỉ còn 1 quân cờ hoặc quân anh hùng.
